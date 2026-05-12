using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenuPanel;
    public RectTransform shakeTarget;
    public ComputerDesktopManager desktopManager;

    public float shakeDuration = 0.25f;
    public float shakeAmount = 10f;

    bool shaking;

    void Start()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }
    }

    public void ToggleMenu()
    {
        if (startMenuPanel == null) return;

        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
    }

    public void CloseMenu()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }
    }

    public void OpenDocuments()
    {
        CloseMenu();

        if (desktopManager != null)
        {
            desktopManager.OpenDocuments();
        }
    }

    public void OpenSettings()
    {
        CloseMenu();

        if (desktopManager != null)
        {
            desktopManager.OpenSettings();
        }
    }

    public void ClickHelp()
    {
        if (!shaking && shakeTarget != null)
        {
            StartCoroutine(ShakeRoutine());
        }
    }

    public void PowerOff()
    {
        CloseMenu();

        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.ExitComputer();
        }
    }

    IEnumerator ShakeRoutine()
    {
        shaking = true;

        Vector2 originalPosition = shakeTarget.anchoredPosition;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float x = Random.Range(-1f, 1f) * shakeAmount;
            shakeTarget.anchoredPosition = originalPosition + new Vector2(x, 0f);

            yield return null;
        }

        shakeTarget.anchoredPosition = originalPosition;
        shaking = false;
    }
}