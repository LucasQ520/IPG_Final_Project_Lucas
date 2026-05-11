using UnityEngine;

public class PlayerClickInteract : MonoBehaviour
{
    public float interactDistance = 5f;

    void Update()
    {
        if (EscapeRoomManager.instance != null)
        {
            if (EscapeRoomManager.instance.IsUsingComputer()) return;
            if (EscapeRoomManager.instance.IsUsingKeypad()) return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                ComputerInteract computer = hit.collider.GetComponent<ComputerInteract>();
                if (computer != null)
                {
                    computer.OpenComputer();
                    return;
                }

                KeypadInteract keypad = hit.collider.GetComponent<KeypadInteract>();
                if (keypad != null)
                {
                    keypad.OpenKeypad();
                    return;
                }
            }
        }
    }
}