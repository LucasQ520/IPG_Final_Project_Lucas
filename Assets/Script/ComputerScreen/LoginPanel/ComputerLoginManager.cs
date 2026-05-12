using UnityEngine;
using TMPro;
using System.Collections;

public class ComputerLoginManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject desktopPanel;

    [Header("Login Input")]
    public TMP_InputField codeInput;
    public string correctComputerCode = "1124";

    [Header("Shake Effect")]
    public RectTransform shakeTarget;
    public float shakeDuration = 0.3f;
    public float shakeAmount = 12f;

    bool shaking;

    void Start()
    {
        if (desktopPanel != null)
        {
            desktopPanel.SetActive(false);
        }

        if (codeInput != null)
        {
            codeInput.text = "";
        }
    }

    void Update()
    {
        if (loginPanel != null && loginPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitComputerCode();
            }
        }
    }

    public void ShowLoginScreen()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }

        if (desktopPanel != null)
        {
            desktopPanel.SetActive(false);
        }

        if (codeInput != null)
        {
            codeInput.text = "";
            codeInput.ActivateInputField();
        }
    }

    public void SubmitComputerCode()
    {
        if (codeInput == null) return;

        string input = codeInput.text.Trim();

        if (EscapeRoomManager.instance != null && input == EscapeRoomManager.instance.generatedComputerCode)
        {
            OpenDesktop();
        }
        else
        {
            codeInput.text = "";

            if (!shaking && shakeTarget != null)
            {
                StartCoroutine(ShakeRoutine());
            }

            codeInput.ActivateInputField();
        }
    }

    IEnumerator ShakeRoutine()
    {
        shaking = true;

        Vector2 originalPosition = shakeTarget.anchoredPosition;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakeAmount;
            shakeTarget.anchoredPosition = originalPosition + new Vector2(x, 0f);

            yield return null;
        }

        shakeTarget.anchoredPosition = originalPosition;
        shaking = false;
    }

    public void OpenDesktop()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }

        if (desktopPanel != null)
        {
            desktopPanel.SetActive(true);
        }
    }

    public void CancelLogin()
    {
        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.ExitComputer();
        }
    }
}