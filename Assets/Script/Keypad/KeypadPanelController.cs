using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadPanelController : MonoBehaviour
{
    [Header("Display")]
    public TMP_InputField codeInput;
    public int maxCodeLength = 6;

    [Header("Shake")]
    public RectTransform shakeTarget;
    public float shakeDuration = 0.25f;
    public float shakeAmount = 10f;

    bool shaking;

    void OnEnable()
    {
        ClearCode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeRoomManager.instance != null)
            {
                EscapeRoomManager.instance.CloseKeypad();
            }
        }
    }

    public void Press1()
    {
        AddNumber("1");
    }

    public void Press2()
    {
        AddNumber("2");
    }

    public void Press3()
    {
        AddNumber("3");
    }

    public void Press4()
    {
        AddNumber("4");
    }

    public void Press5()
    {
        AddNumber("5");
    }

    public void Press6()
    {
        AddNumber("6");
    }

    public void Press7()
    {
        AddNumber("7");
    }

    public void Press8()
    {
        AddNumber("8");
    }

    public void Press9()
    {
        AddNumber("9");
    }

    public void Press0()
    {
        AddNumber("0");
    }

    public void PressStar()
    {
        ClearCode();
    }

    public void PressPound()
    {
        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.SubmitDoorCode();
        }

        if (codeInput != null && codeInput.text != EscapeRoomManager.instance.correctDoorCode)
        {
            if (!shaking && shakeTarget != null)
            {
                StartCoroutine(ShakeRoutine());
            }
        }
    }

    void AddNumber(string number)
    {
        if (codeInput == null) return;
        if (codeInput.text.Length >= maxCodeLength) return;

        codeInput.text += number;
    }

    void ClearCode()
    {
        if (codeInput != null)
        {
            codeInput.text = "";
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
}