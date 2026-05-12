using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public void OpenComputer()
    {
        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.OpenComputer();
        }
    }
}