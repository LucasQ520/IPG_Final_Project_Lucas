using UnityEngine;
using TMPro;
using System;

public class DesktopClock : MonoBehaviour
{
    public TMP_Text clockText;

    void OnEnable()
    {
        UpdateClock();
    }

    void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        if (clockText != null)
        {
            clockText.text = DateTime.Now.ToString("h:mm:ss");
        }
    }
}