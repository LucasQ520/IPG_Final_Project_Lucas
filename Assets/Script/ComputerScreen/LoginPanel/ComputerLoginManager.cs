using UnityEngine;
using TMPro;
using System.Collections;

public class ComputerLoginManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject desktopPanel;

    [Header("Input")]
    public TMP_InputField codeInput;

    [Header("Wrong Code Shake")]
    public RectTransform shakeTarget;
    public float shakeDuration = 0.25f;
    public float shakeAmount = 10f;

    bool shaking;

    void Start()
    {
        ResetToLogin();
    }

    void OnEnable()
    {
        ResetToLogin();
    }

    void Update()
    {
        if (loginPanel != null && loginPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitLoginCode();
            }
        }
    }

    public void ResetToLogin()
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

    public void SubmitLoginCode()
    {
        if (codeInput == null) return;

        string input = codeInput.text.Trim();

        if (ComputerCodeManager.instance != null && ComputerCodeManager.instance.CheckComputerCode(input))
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