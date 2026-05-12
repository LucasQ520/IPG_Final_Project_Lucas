using UnityEngine;
using TMPro;
using System.Collections;

public class DocumentsWindowController : MonoBehaviour
{
    [Header("System Note Text")]
    public TMP_Text systemNoteText;

    [Header("Shake")]
    public RectTransform shakeTarget;
    public float shakeDuration = 0.25f;
    public float shakeAmount = 8f;

    bool shaking;

    void OnEnable()
    {
        SetSystemNote("");
    }

    public void OpenReadMeOrder()
    {
        SetSystemNote("Trust the order of time.");
    }

    public void OpenMonitorLog()
    {
        ShowDigitFile("Monitor_Log.txt", 2);
    }

    public void OpenFinalCheck()
    {
        ShowDigitFile("Final_Check.txt", 5);
    }

    public void OpenLampReport()
    {
        ShowDigitFile("Lamp_Report.txt", 0);
    }

    public void OpenDoorSystem()
    {
        ShowDigitFile("Door_System.txt", 4);
    }

    public void OpenDrawerRecord()
    {
        ShowDigitFile("Drawer_Record.txt", 1);
    }

    public void OpenKeypadNote()
    {
        ShowDigitFile("Keypad_Note.txt", 3);
    }

    void ShowDigitFile(string fileName, int digitIndex)
    {
        if (DoorCodeManager.instance == null)
        {
            SetSystemNote(fileName + ": The file is unreadable.");
            return;
        }

        string digit = DoorCodeManager.instance.GetDigit(digitIndex);
        SetSystemNote("The file contains number \"" + digit + "\".");
    }

    void SetSystemNote(string message)
    {
        if (systemNoteText != null)
        {
            systemNoteText.text = message;
        }
    }

    public void ClickBrokenMenu()
    {
        if (!shaking && shakeTarget != null)
        {
            StartCoroutine(ShakeRoutine());
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