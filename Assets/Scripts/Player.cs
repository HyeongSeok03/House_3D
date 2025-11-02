using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.8f; // 🔸 달리기 속도 배율
    [SerializeField] private float jumpForce = 6f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private IInteractable interactable;
    [SerializeField] private GameObject interactableUI;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool isSprinting; // 🔸 Shift 입력 상태
    private float xRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Physics.gravity = new Vector3(0, -20f, 0);
    }

    void Update()
    {
        HandleLook();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMove();
        HandleInteract();
    }

    // === Input System 콜백 ===
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed; // 🔸 Shift 누를 때 true, 뗄 때 false
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        Debug.Log("Try interact.");
        if (interactable != null)
        {
            interactable.Interact(this);
            return;
        }
        Debug.Log("can not interact.");
    }

    // === 이동 ===
    private void HandleMove()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 move = transform.TransformDirection(moveDir);

        float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
        rb.linearVelocity = new Vector3(move.x * currentSpeed, rb.linearVelocity.y, move.z * currentSpeed);
    }

    private void HandleInteract()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            interactable = hit.collider.GetComponent<IInteractable>();
            interactableUI.SetActive(true);
            return;
        }
        interactable = null;
        interactableUI.SetActive(false);
    }

    // === 점프 ===
    private void HandleJump()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (jumpPressed && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPressed = false;
        }
    }

    // === 마우스 시점 회전 ===
    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}
