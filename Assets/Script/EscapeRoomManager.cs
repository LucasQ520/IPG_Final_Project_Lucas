using UnityEngine;
using TMPro;
using System.Collections;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager instance;

    [Header("Player / Camera")]
    public Transform playerCamera;
    public FirstPersonController playerController;
    public Transform computerViewTarget;

    [Header("Computer UI")]
    public GameObject computerCanvas;

    [Header("HUD")]
    public GameObject crosshair;

    [Header("Door Puzzle")]
    public TMP_InputField doorCodeInput;
    public TMP_Text doorMessageText;
    public string correctDoorCode = "7392";
    public Transform door;
    public Vector3 doorOpenRotation = new Vector3(0f, 90f, 0f);
    public float doorOpenDuration = 1f;

    [Header("Camera Zoom Settings")]
    public float zoomDuration = 0.7f;

    Vector3 originalCameraPosition;
    Quaternion originalCameraRotation;

    bool usingComputer;
    bool movingCamera;
    bool doorOpened;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (computerCanvas != null)
        {
            computerCanvas.SetActive(false);
        }

        if (doorMessageText != null)
        {
            doorMessageText.text = "";
        }

        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnterComputer()
    {
        if (usingComputer) return;
        if (movingCamera) return;
        if (playerCamera == null) return;
        if (computerViewTarget == null) return;
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        usingComputer = true;

        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }

        StartCoroutine(MoveCameraRoutine(
            playerCamera.position,
            playerCamera.rotation,
            computerViewTarget.position,
            computerViewTarget.rotation,
            true
        ));
    }

    public void ExitComputer()
    {
        if (!usingComputer) return;
        if (movingCamera) return;
        if (playerCamera == null) return;
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (computerCanvas != null)
        {
            computerCanvas.SetActive(false);
        }

        StartCoroutine(MoveCameraRoutine(
            playerCamera.position,
            playerCamera.rotation,
            originalCameraPosition,
            originalCameraRotation,
            false
        ));
    }

    IEnumerator MoveCameraRoutine(
        Vector3 startPosition,
        Quaternion startRotation,
        Vector3 endPosition,
        Quaternion endRotation,
        bool showComputerAfterMove)
    {
        movingCamera = true;

        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;

            float t = timer / zoomDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            playerCamera.position = Vector3.Lerp(startPosition, endPosition, t);
            playerCamera.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        playerCamera.position = endPosition;
        playerCamera.rotation = endRotation;

        if (showComputerAfterMove)
        {
            if (computerCanvas != null)
            {
                computerCanvas.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            usingComputer = false;

            if (playerController != null)
            {
                playerController.enabled = true;
            }

            if (crosshair != null)
            {
                crosshair.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        movingCamera = false;
    }

    public void SubmitDoorCode()
    {
        if (doorOpened) return;
        if (doorCodeInput == null) return;

        string input = doorCodeInput.text.Trim();

        if (input == correctDoorCode)
        {
            OpenDoor();
        }
        else
        {
            if (doorMessageText != null)
            {
                doorMessageText.text = "Wrong code.";
            }

            doorCodeInput.text = "";
        }
    }

    void OpenDoor()
    {
        doorOpened = true;

        if (doorMessageText != null)
        {
            doorMessageText.text = "Door unlocked.";
        }

        if (door != null)
        {
            StartCoroutine(OpenDoorRoutine());
        }
    }

    IEnumerator OpenDoorRoutine()
    {
        Quaternion startRotation = door.localRotation;
        Quaternion endRotation = Quaternion.Euler(doorOpenRotation);

        float timer = 0f;

        while (timer < doorOpenDuration)
        {
            timer += Time.deltaTime;

            float t = timer / doorOpenDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            door.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        door.localRotation = endRotation;
    }

    public bool IsUsingComputer()
    {
        return usingComputer;
    }

    public bool IsDoorOpened()
    {
        return doorOpened;
    }
}