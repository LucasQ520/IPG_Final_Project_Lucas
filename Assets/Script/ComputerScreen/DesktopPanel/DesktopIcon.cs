using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DesktopIcon : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
    public float doubleClickTime = 0.3f;
    public UnityEvent onDoubleClick;

    RectTransform rectTransform;
    Canvas canvas;
    float lastClickTime = -1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
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
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}