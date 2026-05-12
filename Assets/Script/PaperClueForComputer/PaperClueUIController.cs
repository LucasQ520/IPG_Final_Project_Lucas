using UnityEngine;

public class PaperClueUIController : MonoBehaviour
{
    public static GameObject currentPaperObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeRoomManager.instance != null)
            {
                EscapeRoomManager.instance.ClosePaperClue();
            }
        }
    }
}