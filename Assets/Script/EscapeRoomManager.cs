using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager instance;

    [Header("Scene")]
    public string endingSceneName = "EndingScene";

    [Header("Player")]
    public MonoBehaviour playerController;
    public Transform playerCamera;
    public GameObject crosshair;

    [Header("Camera View Points")]
    public Transform computerViewPoint;
    public Transform keypadViewPoint;

    [Header("UI Panels")]
    public GameObject computerPanel;
    public GameObject keypadPanel;
    public GameObject paperPanel;

    [Header("Computer Effect")]
    public MonitorStaticEffect monitorStaticEffect;

    [Header("Paper Clue")]
    public TMP_Text paperCodeText;

    [Header("Camera Movement")]
    public float cameraMoveDuration = 0.8f;

    bool usingComputer;
    bool usingKeypad;
    bool readingPaper;
    bool cameraMoving;

    Vector3 savedCameraPosition;
    Quaternion savedCameraRotation;

    GameObject currentPaperObject;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (computerPanel != null)
        {
            computerPanel.SetActive(false);
        }

        if (keypadPanel != null)
        {
            keypadPanel.SetActive(false);
        }

        if (paperPanel != null)
        {
            paperPanel.SetActive(false);
        }

        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (usingKeypad)
            {
                ExitKeypad();
            }
            else if (usingComputer)
            {
                ExitComputer();
            }
            else if (readingPaper)
            {
                ClosePaperClue();
            }
        }
    }

    public void OpenComputer()
    {
        if (cameraMoving) return;
        if (usingComputer || usingKeypad || readingPaper) return;
        if (playerCamera == null) return;

        usingComputer = true;
        FreezePlayer(true);

        if (computerViewPoint != null)
        {
            StartCoroutine(MoveCameraToView(computerViewPoint, true));
        }
        else
        {
            OpenComputerPanel();
        }
    }

    public void EnterComputer()
    {
        OpenComputer();
    }

    public void ExitComputer()
    {
        if (cameraMoving) return;
        if (!usingComputer) return;

        usingComputer = false;

        if (computerPanel != null)
        {
            computerPanel.SetActive(false);
        }

        if (playerCamera != null)
        {
            StartCoroutine(RestoreCamera());
        }
        else
        {
            FreezePlayer(false);
        }
    }

    public void OpenKeypad()
    {
        if (cameraMoving) return;
        if (usingComputer || usingKeypad || readingPaper) return;
        if (playerCamera == null) return;

        usingKeypad = true;
        FreezePlayer(true);

        if (keypadViewPoint != null)
        {
            StartCoroutine(MoveCameraToView(keypadViewPoint, false));
        }
        else
        {
            OpenKeypadPanel();
        }
    }

    public void ExitKeypad()
    {
        if (cameraMoving) return;
        if (!usingKeypad) return;

        usingKeypad = false;

        if (keypadPanel != null)
        {
            keypadPanel.SetActive(false);
        }

        if (playerCamera != null)
        {
            StartCoroutine(RestoreCamera());
        }
        else
        {
            FreezePlayer(false);
        }
    }

    public void CloseKeypad()
    {
        ExitKeypad();
    }

    public void OpenPaperClue(GameObject paperObject)
    {
        if (cameraMoving) return;
        if (usingComputer || usingKeypad || readingPaper) return;

        readingPaper = true;
        currentPaperObject = paperObject;

        FreezePlayer(true);

        if (paperCodeText != null && ComputerCodeManager.instance != null)
        {
            paperCodeText.text = ComputerCodeManager.instance.GetComputerCode();
        }

        if (paperPanel != null)
        {
            paperPanel.SetActive(true);
        }
    }

    public void ClosePaperClue()
    {
        if (!readingPaper) return;

        readingPaper = false;

        if (paperPanel != null)
        {
            paperPanel.SetActive(false);
        }

        if (currentPaperObject != null)
        {
            currentPaperObject.SetActive(false);
            currentPaperObject = null;
        }

        FreezePlayer(false);
    }

    public void GoToEndingScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(endingSceneName);
    }

    IEnumerator MoveCameraToView(Transform viewPoint, bool openingComputer)
    {
        cameraMoving = true;

        savedCameraPosition = playerCamera.position;
        savedCameraRotation = playerCamera.rotation;

        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        Vector3 endPosition = viewPoint.position;
        Quaternion endRotation = viewPoint.rotation;

        float timer = 0f;

        while (timer < cameraMoveDuration)
        {
            timer += Time.deltaTime;

            float t = timer / cameraMoveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            playerCamera.position = Vector3.Lerp(startPosition, endPosition, t);
            playerCamera.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        playerCamera.position = endPosition;
        playerCamera.rotation = endRotation;

        if (openingComputer)
        {
            OpenComputerPanel();
        }
        else
        {
            OpenKeypadPanel();
        }

        cameraMoving = false;
    }

    IEnumerator RestoreCamera()
    {
        cameraMoving = true;

        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        Vector3 endPosition = savedCameraPosition;
        Quaternion endRotation = savedCameraRotation;

        float timer = 0f;

        while (timer < cameraMoveDuration)
        {
            timer += Time.deltaTime;

            float t = timer / cameraMoveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            playerCamera.position = Vector3.Lerp(startPosition, endPosition, t);
            playerCamera.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        playerCamera.position = endPosition;
        playerCamera.rotation = endRotation;

        FreezePlayer(false);

        cameraMoving = false;
    }

    void OpenComputerPanel()
    {
        if (computerPanel != null)
        {
            computerPanel.SetActive(true);
        }

        if (monitorStaticEffect != null)
        {
            monitorStaticEffect.PlayEffect();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OpenKeypadPanel()
    {
        if (keypadPanel != null)
        {
            keypadPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FreezePlayer(bool frozen)
    {
        if (playerController != null)
        {
            playerController.enabled = !frozen;
        }

        if (crosshair != null)
        {
            crosshair.SetActive(!frozen);
        }

        if (frozen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool IsUsingComputer()
    {
        return usingComputer;
    }

    public bool IsUsingKeypad()
    {
        return usingKeypad;
    }

    public bool IsReadingPaper()
    {
        return readingPaper;
    }
}