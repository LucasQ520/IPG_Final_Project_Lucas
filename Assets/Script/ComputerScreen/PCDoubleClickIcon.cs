using UnityEngine;
using UnityEngine.Events;

public class PCDoubleClickIcon : MonoBehaviour
{
    public float doubleClickThreshold = 0.3f;
    public UnityEvent onDoubleClick;

    float lastClickTime = -1f;

    public void OnClickIcon()
    {
        float t = Time.unscaledTime;
        if (lastClickTime > 0f && t - lastClickTime <= doubleClickThreshold)
        {
            lastClickTime = -1f;
            if (onDoubleClick != null) onDoubleClick.Invoke();
        }
        else
        {
            lastClickTime = t;
        }
    }
}
