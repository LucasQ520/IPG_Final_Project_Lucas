using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

public class DesktopIcon : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Icon Info")]
    public string iconName;
    public Sprite iconSprite;
    public bool canRecycle = true;

    [Header("Double Click")]
    public float doubleClickTime = 0.3f;
    public UnityEvent onDoubleClick;

    [Header("Starting Grid Position")]
    public int startColumn = 0;
    public int startRow = 0;
    public bool setStartPositionOnStart = true;

    [Header("Grid Snap")]
    public bool snapToGrid = true;
    public Vector2 gridOrigin = new Vector2(90f, -90f);
    public Vector2 gridSpacing = new Vector2(170f, 170f);

    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Transform desktopParent;
    RectTransform desktopParentRect;
    float lastClickTime = -1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        desktopParent = transform.parent;
        desktopParentRect = desktopParent as RectTransform;
    }

    void Start()
    {
        if (setStartPositionOnStart)
        {
            rectTransform.anchoredPosition = GridToPosition(startColumn, startRow);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.unscaledTime;

        if (lastClickTime > 0f && currentTime - lastClickTime <= doubleClickTime)
        {
            lastClickTime = -1f;

            if (onDoubleClick != null)
            {
                onDoubleClick.Invoke();
            }
        }
        else
        {
            lastClickTime = currentTime;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool droppedInRecycleBin = TryDropIntoRecycleBin();

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        if (droppedInRecycleBin)
        {
            transform.SetParent(desktopParent, false);
            return;
        }

        transform.SetParent(desktopParent, true);

        if (snapToGrid)
        {
            SnapToGrid();
        }
    }

    bool TryDropIntoRecycleBin()
    {
        if (!canRecycle) return false;
        if (EventSystem.current == null) return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        for (int i = 0; i < results.Count; i++)
        {
            RecycleDropTarget target = results[i].gameObject.GetComponentInParent<RecycleDropTarget>();

            if (target != null)
            {
                target.DropIcon(this);
                return true;
            }
        }

        return false;
    }

    public void RestoreToDesktop()
    {
        gameObject.SetActive(true);
        transform.SetParent(desktopParent, false);
        transform.SetAsLastSibling();

        if (snapToGrid)
        {
            SnapToGrid();
        }
    }

    public void SnapBackToGrid()
    {
        if (snapToGrid)
        {
            SnapToGrid();
        }
    }

    void SnapToGrid()
    {
        Vector2 position = rectTransform.anchoredPosition;

        int column = Mathf.RoundToInt((position.x - gridOrigin.x) / gridSpacing.x);
        int row = Mathf.RoundToInt((position.y - gridOrigin.y) / -gridSpacing.y);

        rectTransform.anchoredPosition = GridToPosition(column, row);
    }

    Vector2 GridToPosition(int column, int row)
    {
        float x = gridOrigin.x + column * gridSpacing.x;
        float y = gridOrigin.y - row * gridSpacing.y;

        return new Vector2(x, y);
    }
}