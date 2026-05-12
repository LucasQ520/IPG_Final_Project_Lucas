using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float crouchSpeed = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float standingHeight = 2f;
    public float crouchingHeight = 1.1f;
    public float standingCameraY = 0.7f;
    public float crouchingCameraY = 0.15f;
    public float crouchTransitionSpeed = 8f;

    CharacterController controller;
    Vector3 velocity;
    float cameraPitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.height = standingHeight;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookAround();
        MovePlayer();
        HandleCrouch();
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool crouching = Input.GetKey(crouchKey);
        float currentSpeed = crouching ? crouchSpeed : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        bool crouching = Input.GetKey(crouchKey);

        float targetHeight = crouching ? crouchingHeight : standingHeight;
        float targetCameraY = crouching ? crouchingCameraY : standingCameraY;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * crouchTransitionSpeed
        );

        Vector3 cameraPosition = playerCamera.localPosition;
        cameraPosition.y = Mathf.Lerp(
            cameraPosition.y,
            targetCameraY,
            Time.deltaTime * crouchTransitionSpeed
        );

        playerCamera.localPosition = cameraPosition;
    }
}