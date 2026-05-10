using UnityEngine;
using TMPro;

public class LockedFolderUI : MonoBehaviour
{
    public GameObject lockedFolderPanel;
    public GameObject folderContentPanel;
    public TMP_InputField passwordInput;
    public RectTransform shakeTarget;
    public string correctPassword = "2024/11/24";
    public float shakeDuration = 0.3f;
    public float shakeAmount = 10f;

    bool shaking;

    public void OpenPrompt()
    {
        if (lockedFolderPanel != null)
        {
            lockedFolderPanel.SetActive(true);
            if (passwordInput != null) passwordInput.text = "";
        }
    }

    public void OnClickOk()
    {
        if (passwordInput == null) return;
        string input = passwordInput.text.Trim();
        if (IsPasswordCorrect(input))
        {
            if (lockedFolderPanel != null) lockedFolderPanel.SetActive(false);
            if (folderContentPanel != null) folderContentPanel.SetActive(true);
        }
        else
        {
            if (!shaking && shakeTarget != null)
            {
                StartCoroutine(ShakeRoutine());
            }
        }
    }

    public void OnClickCancel()
    {
        if (lockedFolderPanel != null) lockedFolderPanel.SetActive(false);
    }

    bool IsPasswordCorrect(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        if (input == correctPassword) return true;
        if (input == "2024-11-24") return true;
        if (input == "2024.11.24") return true;
        return false;
    }

    System.Collections.IEnumerator ShakeRoutine()
    {
        shaking = true;
        Vector2 originalPos = shakeTarget.anchoredPosition;
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * shakeAmount;
            shakeTarget.anchoredPosition = originalPos + new Vector2(x, 0f);
            yield return null;
        }
        shakeTarget.anchoredPosition = originalPos;
        shaking = false;
    }
}
