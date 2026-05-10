using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragHandle : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform window;

    Vector2 pointerOffset;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        pointerOffset = (Vector2)window.position - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        window.position = mousePos + pointerOffset;
    }
}
