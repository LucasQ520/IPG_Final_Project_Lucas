using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PCDesktopManager : MonoBehaviour
{
    public GameObject folderAWindow;
    public GameObject folderBWindow;
    public GameObject photoWindow;
    public Image photoImage;
    public Sprite selfieSprite;

    public GameObject lockedWindow;
    public TMP_InputField passwordInput;
    public string correctPassword = "20241124";
    public TMP_Text wrongPasswordText;

    public GameObject lockedContentWindow;

    public GameObject gameWindow;

    public void OpenFolderA()
    {
        if (folderAWindow != null) folderAWindow.SetActive(true);
    }

    public void CloseFolderA()
    {
        if (folderAWindow != null) folderAWindow.SetActive(false);
    }

    public void OpenFolderB()
    {
        if (folderBWindow != null) folderBWindow.SetActive(true);
    }

    public void CloseFolderB()
    {
        if (folderBWindow != null) folderBWindow.SetActive(false);
    }

    public void OpenSelfiePhoto()
    {
        if (photoWindow != null) photoWindow.SetActive(true);
        if (photoImage != null)
        {
            if (selfieSprite != null)
            {
                photoImage.sprite = selfieSprite;
                photoImage.enabled = true;
            }
            else photoImage.enabled = false;
        }
    }

    public void ClosePhoto()
    {
        if (photoWindow != null) photoWindow.SetActive(false);
    }

    public void OpenLockedPrompt()
    {
        if (wrongPasswordText != null) wrongPasswordText.text = "";
        if (passwordInput != null) passwordInput.text = "";
        if (lockedWindow != null) lockedWindow.SetActive(true);
    }

    public void SubmitPassword()
    {
        if (passwordInput == null) return;
        string input = passwordInput.text;
        if (input == correctPassword)
        {
            if (lockedWindow != null) lockedWindow.SetActive(false);
            OpenLockedContent();
        }
        else
        {
            if (wrongPasswordText != null) wrongPasswordText.text = "Incorrect password.";
            passwordInput.text = "";
        }
    }

    public void OpenLockedContent()
    {
        if (lockedContentWindow != null) lockedContentWindow.SetActive(true);
    }

    public void CloseLockedContent()
    {
        if (lockedContentWindow != null) lockedContentWindow.SetActive(false);
    }

    public void OpenGame()
    {
        if (gameWindow != null) gameWindow.SetActive(true);
    }

    public void CloseGame()
    {
        if (gameWindow != null) gameWindow.SetActive(false);
    }

    public void ClosePC()
    {
        gameObject.SetActive(false);
    }
}
