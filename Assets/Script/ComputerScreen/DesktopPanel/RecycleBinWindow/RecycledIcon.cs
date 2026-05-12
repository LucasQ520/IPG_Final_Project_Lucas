using UnityEngine;
using UnityEngine.EventSystems;

public class RecycledIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    DesktopIcon originalIcon;
    RecycleBinManager recycleBinManager;

    RectTransform rectTransform;
    RectTransform recycleArea;
    Canvas canvas;
    CanvasGroup canvasGroup;

    Vector2 startPosition;
    Transform startParent;

    public void Setup(DesktopIcon icon, RecycleBinManager manager)
    {
        originalIcon = icon;
        recycleBinManager = manager;
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        startParent = transform.parent;
        recycleArea = startParent as RectTransform;

        if (canvas != null)
        {
            transform.SetParent(canvas.transform, true);
            transform.SetAsLastSibling();
        }

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
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        if (DroppedOutsideRecycleArea(eventData))
        {
            if (recycleBinManager != null)
            {
                recycleBinManager.RestoreIcon(originalIcon, gameObject);
            }

            return;
        }

        transform.SetParent(startParent, false);
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = startPosition;
    }

    bool DroppedOutsideRecycleArea(PointerEventData eventData)
    {
        if (recycleArea == null) return true;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            recycleArea,
            Input.mousePosition,
            canvas != null ? canvas.worldCamera : null
        );

        return !inside;
    }
}