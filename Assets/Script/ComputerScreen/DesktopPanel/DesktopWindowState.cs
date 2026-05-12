using UnityEngine;

public class DesktopWindowState : MonoBehaviour
{
    public RectTransform windowRect;
    public Vector2 maximizedSize = new Vector2(1600f, 850f);
    public Vector2 maximizedPosition = new Vector2(0f, 20f);

    bool maximized;

    Vector2 originalSize;
    Vector2 originalPosition;

    void Awake()
    {
        if (windowRect == null)
        {
            windowRect = GetComponent<RectTransform>();
        }
    }

    public void ToggleMaximize()
    {
        if (windowRect == null) return;

        if (!maximized)
        {
            originalSize = windowRect.sizeDelta;
            originalPosition = windowRect.anchoredPosition;

            windowRect.SetAsLastSibling();
            windowRect.sizeDelta = maximizedSize;
            windowRect.anchoredPosition = maximizedPosition;

            maximized = true;
        }
        else
        {
            windowRect.sizeDelta = originalSize;
            windowRect.anchoredPosition = originalPosition;

            maximized = false;
        }
    }

    public bool IsMaximized()
    {
        return maximized;
    }
}