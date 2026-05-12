using UnityEngine;

public class PaperClueInteract : MonoBehaviour
{
    public void OpenPaper()
    {
        if (EscapeRoomManager.instance != null)
        {
            EscapeRoomManager.instance.OpenPaperClue(gameObject);
        }
    }
}