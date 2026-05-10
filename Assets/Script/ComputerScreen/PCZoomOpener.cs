using UnityEngine;
using UnityEngine.UI;

public class PCZoomToTargetWorld : MonoBehaviour
{
    public Transform dormRoot;
    public Transform pcTarget;
    public GameObject pcPanel;
    public Button openPcButton;

    public float zoomDuration = 0.6f;
    public float zoomScale = 1.6f;

    Vector3 originalLocalPos;
    Vector3 originalScale;
    bool running;
    bool zoomedIn;

    void Awake()
    {
        if (dormRoot != null)
        {
            originalLocalPos = dormRoot.localPosition;
            originalScale = dormRoot.localScale;
        }
    }

    public void OnClickOpenPC()
    {
        if (running) return;
        if (zoomedIn) return;
        if (dormRoot == null || pcTarget == null || pcPanel == null) return;

        running = true;
        if (openPcButton != null) openPcButton.interactable = false;

        StartCoroutine(ZoomInRoutine());
    }

    public void OnClickClosePC()
    {
        if (running) return;
        if (!zoomedIn) return;
        if (dormRoot == null) return;

        running = true;
        if (pcPanel != null) pcPanel.SetActive(false);

        StartCoroutine(ZoomOutRoutine());
    }

    System.Collections.IEnumerator ZoomInRoutine()
    {
        Vector3 targetLocalPos = pcTarget.localPosition;
        Vector3 targetPos = -targetLocalPos * zoomScale;

        Vector3 startPos = dormRoot.localPosition;
        Vector3 startScale = dormRoot.localScale;
        Vector3 endScale = Vector3.one * zoomScale;

        float t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / zoomDuration);
            dormRoot.localScale = Vector3.Lerp(startScale, endScale, n);
            dormRoot.localPosition = Vector3.Lerp(startPos, targetPos, n);
            yield return null;
        }

        dormRoot.localScale = endScale;
        dormRoot.localPosition = targetPos;

        if (pcPanel != null) pcPanel.SetActive(true);

        zoomedIn = true;
        running = false;
        if (openPcButton != null) openPcButton.gameObject.SetActive(false);
    }

    System.Collections.IEnumerator ZoomOutRoutine()
    {
        Vector3 startPos = dormRoot.localPosition;
        Vector3 startScale = dormRoot.localScale;

        float t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / zoomDuration);
            dormRoot.localScale = Vector3.Lerp(startScale, originalScale, n);
            dormRoot.localPosition = Vector3.Lerp(startPos, originalLocalPos, n);
            yield return null;
        }

        dormRoot.localScale = originalScale;
        dormRoot.localPosition = originalLocalPos;

        zoomedIn = false;
        running = false;

        if (openPcButton != null)
        {
            openPcButton.gameObject.SetActive(true);
            openPcButton.interactable = true;
        }
    }
}
