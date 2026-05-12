using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopWindowDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform window;
    public DesktopWindowState windowState;

    Vector2 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (window == null) return;

        window.SetAsLastSibling();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            window,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (window == null) return;
        if (window.parent == null) return;
        if (windowState != null && windowState.IsMaximized()) return;

        RectTransform parentRect = window.parent as RectTransform;

        Vector2 localMousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePosition
        );

        window.localPosition = localMousePosition - offset;
    }
}