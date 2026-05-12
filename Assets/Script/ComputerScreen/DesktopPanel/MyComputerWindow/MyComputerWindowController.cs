using UnityEngine;
using TMPro;

public class MyComputerWindowController : MonoBehaviour
{
    public TMP_Text statusText;

    void OnEnable()
    {
        SetStatus("6 object(s)");
    }

    public void ClickFile()
    {
        SetStatus("No file selected.");
    }

    public void ClickEdit()
    {
        SetStatus("This action is not available.");
    }

    public void ClickView()
    {
        SetStatus("Icons are already displayed.");
    }

    public void ClickHelp()
    {
        SetStatus("Help files are missing.");
    }

    public void ClickFloppy()
    {
        SetStatus("Please insert a disk into drive A:");
    }

    public void ClickSystem()
    {
        SetStatus("Access denied.");
    }

    public void ClickData()
    {
        SetStatus("Drive D: is empty.");
    }

    public void ClickCDROM()
    {
        SetStatus("No disc detected.");
    }

    public void ClickControlPanel()
    {
        SetStatus("Control Panel is unavailable.");
    }

    public void ClickNetwork()
    {
        SetStatus("Network unavailable.");
    }

    void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}