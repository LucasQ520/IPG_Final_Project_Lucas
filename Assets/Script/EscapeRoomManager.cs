using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager instance;

    [Header("Player / Camera")]
    public Transform playerCamera;
    public FirstPersonController playerController;

    [Header("Computer")]
    public Transform computerViewTarget;
    public GameObject computerCanvas;
    public MonitorStaticEffect monitorStaticEffect;

    [Header("Keypad")]
    public Transform keypadViewTarget;
    public GameObject doorKeypadPanel;

    [Header("HUD")]
    public GameObject crosshair;

    [Header("Door Puzzle")]
    public TMP_InputField doorCodeInput;
    public TMP_Text doorMessageText;
    public string correctDoorCode = "7392";

    [Header("Ending")]
    public string endingSceneName = "EndingScene";

    [Header("Camera Zoom Settings")]
    public float zoomDuration = 0.7f;

    Vector3 originalCameraPosition;
    Quaternion originalCameraRotation;

    bool usingComputer;
    bool usingKeypad;
    bool movingCamera;
    bool gameEnded;

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

        if (doorKeypadPanel != null)
        {
            doorKeypadPanel.SetActive(false);
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
        if (usingComputer || usingKeypad) return;
        if (movingCamera) return;
        if (gameEnded) return;
        if (playerCamera == null || computerViewTarget == null) return;

        usingComputer = true;

        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;

        FreezePlayer();

        StartCoroutine(MoveCameraRoutine(
            playerCamera.position,
            playerCamera.rotation,
            computerViewTarget.position,
            computerViewTarget.rotation,
            true,
            false
        ));
    }

    public void ExitComputer()
    {
        if (!usingComputer) return;
        if (movingCamera) return;

        if (computerCanvas != null)
        {
            computerCanvas.SetActive(false);
        }

        StartCoroutine(ReturnCameraRoutine());
    }

    public void OpenKeypad()
    {
        if (usingComputer || usingKeypad) return;
        if (movingCamera) return;
        if (gameEnded) return;
        if (playerCamera == null || keypadViewTarget == null) return;

        usingKeypad = true;

        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;

        FreezePlayer();

        if (doorMessageText != null)
        {
            doorMessageText.text = "";
        }

        if (doorCodeInput != null)
        {
            doorCodeInput.text = "";
        }

        StartCoroutine(MoveCameraRoutine(
            playerCamera.position,
            playerCamera.rotation,
            keypadViewTarget.position,
            keypadViewTarget.rotation,
            false,
            true
        ));
    }

    public void CloseKeypad()
    {
        if (!usingKeypad) return;
        if (movingCamera) return;

        if (doorKeypadPanel != null)
        {
            doorKeypadPanel.SetActive(false);
        }

        StartCoroutine(ReturnCameraRoutine());
    }

    void FreezePlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void UnfreezePlayer()
    {
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

    IEnumerator MoveCameraRoutine(
        Vector3 startPosition,
        Quaternion startRotation,
        Vector3 endPosition,
        Quaternion endRotation,
        bool openComputerAfterMove,
        bool openKeypadAfterMove)
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

        if (openComputerAfterMove && computerCanvas != null)
        {
            computerCanvas.SetActive(true);

            if (monitorStaticEffect != null)
            {
                monitorStaticEffect.PlayEffect();
            }
        }

        if (openKeypadAfterMove && doorKeypadPanel != null)
        {
            doorKeypadPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        movingCamera = false;
    }

    IEnumerator ReturnCameraRoutine()
    {
        movingCamera = true;

        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;

            float t = timer / zoomDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            playerCamera.position = Vector3.Lerp(startPosition, originalCameraPosition, t);
            playerCamera.rotation = Quaternion.Slerp(startRotation, originalCameraRotation, t);

            yield return null;
        }

        playerCamera.position = originalCameraPosition;
        playerCamera.rotation = originalCameraRotation;

        usingComputer = false;
        usingKeypad = false;

        UnfreezePlayer();

        movingCamera = false;
    }

    public void SubmitDoorCode()
    {
        if (gameEnded) return;
        if (doorCodeInput == null) return;

        string input = doorCodeInput.text.Trim();

        if (input == correctDoorCode)
        {
            GoToEndingScene();
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

    void GoToEndingScene()
    {
        gameEnded = true;

        if (doorMessageText != null)
        {
            doorMessageText.text = "Access granted.";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(endingSceneName);
    }

    public bool IsUsingComputer()
    {
        return usingComputer;
    }

    public bool IsUsingKeypad()
    {
        return usingKeypad;
    }
}