using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadPanelController : MonoBehaviour
{
    [Header("Display")]
    public TMP_InputField codeInput;
    public int maxDigits = 6;

    [Header("Wrong Code Shake")]
    public RectTransform shakeTarget;
    public float shakeDuration = 0.25f;
    public float shakeAmount = 10f;

    string currentInput = "";
    bool shaking;

    void OnEnable()
    {
        ClearInput();
    }

    public void PressDigit(string digit)
    {
        if (currentInput.Length >= maxDigits) return;

        currentInput += digit;
        UpdateDisplay();
    }

    public void PressZero()
    {
        PressDigit("0");
    }

    public void PressOne()
    {
        PressDigit("1");
    }

    public void PressTwo()
    {
        PressDigit("2");
    }

    public void PressThree()
    {
        PressDigit("3");
    }

    public void PressFour()
    {
        PressDigit("4");
    }

    public void PressFive()
    {
        PressDigit("5");
    }

    public void PressSix()
    {
        PressDigit("6");
    }

    public void PressSeven()
    {
        PressDigit("7");
    }

    public void PressEight()
    {
        PressDigit("8");
    }

    public void PressNine()
    {
        PressDigit("9");
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    public void SubmitCode()
    {
        if (DoorCodeManager.instance != null && DoorCodeManager.instance.CheckDoorCode(currentInput))
        {
            if (EscapeRoomManager.instance != null)
            {
                EscapeRoomManager.instance.GoToEndingScene();
            }
        }
        else
        {
            ClearInput();

            if (!shaking && shakeTarget != null)
            {
                StartCoroutine(ShakeRoutine());
            }
        }
    }

    void UpdateDisplay()
    {
        if (codeInput != null)
        {
            codeInput.text = currentInput;
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