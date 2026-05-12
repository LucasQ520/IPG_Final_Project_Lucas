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
            if (EscapeRoomManager.instance.IsReadingPaper()) return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            for (int i = 0; i < hits.Length; i++)
            {
                ComputerInteract computer = hits[i].collider.GetComponent<ComputerInteract>();

                if (computer != null)
                {
                    computer.OpenComputer();
                    return;
                }

                KeypadInteract keypad = hits[i].collider.GetComponentInParent<KeypadInteract>();

                if (keypad != null)
                {
                    keypad.OpenKeypad();
                    return;
                }

                PaperClueInteract paper = hits[i].collider.GetComponent<PaperClueInteract>();

                if (paper != null)
                {
                    paper.OpenPaper();
                    return;
                }

                LightSwitchInteract lightSwitch = hits[i].collider.GetComponent<LightSwitchInteract>();

                if (lightSwitch != null)
                {
                    lightSwitch.ToggleLight();
                    return;
                }
            }
        }
    }
}