using UnityEngine;

public class KeypadInteract : MonoBehaviour
{
    public void OpenKeypad()
    {
        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.OpenKeypad();
        }
    }
}