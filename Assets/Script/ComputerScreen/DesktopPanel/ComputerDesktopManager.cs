using UnityEngine;

public class ComputerDesktopManager : MonoBehaviour
{
    [Header("Windows")]
    public GameObject myComputerWindow;
    public GameObject documentsWindow;
    public GameObject settingsWindow;
    public GameObject notesWindow;
    public GameObject recycleBinWindow;

    void Start()
    {
        CloseAllWindows();
    }

    public void OpenMyComputer()
    {
        OpenWindow(myComputerWindow);
    }

    public void OpenDocuments()
    {
        OpenWindow(documentsWindow);
    }

    public void OpenSettings()
    {
        OpenWindow(settingsWindow);
    }

    public void OpenNotes()
    {
        OpenWindow(notesWindow);
    }

    public void OpenRecycleBin()
    {
        OpenWindow(recycleBinWindow);
    }

    public void CloseMyComputer()
    {
        CloseWindow(myComputerWindow);
    }

    public void CloseDocuments()
    {
        CloseWindow(documentsWindow);
    }

    public void CloseSettings()
    {
        CloseWindow(settingsWindow);
    }

    public void CloseNotes()
    {
        CloseWindow(notesWindow);
    }

    public void CloseRecycleBin()
    {
        CloseWindow(recycleBinWindow);
    }

    public void CloseAllWindows()
    {
        CloseWindow(myComputerWindow);
        CloseWindow(documentsWindow);
        CloseWindow(settingsWindow);
        CloseWindow(notesWindow);
        CloseWindow(recycleBinWindow);
    }

    void OpenWindow(GameObject window)
    {
        if (window == null) return;

        window.SetActive(true);
        window.transform.SetAsLastSibling();
    }

    void CloseWindow(GameObject window)
    {
        if (window == null) return;

        window.SetActive(false);
    }
}