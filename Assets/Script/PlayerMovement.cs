using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Rigidbody _rigidbody;
    private bool isGrounded;
    public Transform groundCheck;
    private float groundCheckRadius;
    public LayerMask HexTileLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Camera playerCamera;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpDuration = 2f;
    [SerializeField] private float jumpDistance = 5f;
    private bool isJumping = false;
    private Vector3 moveDirection;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        _rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCamera = Camera.main;
    }
    private void Start()
    {
        groundCheckRadius = groundCheck.localScale.y;
    }

    private void OnEnable()
    {
        controls.Player1.Enable();
        controls.Player1.Jump.performed += _ => Jump();
    }

    private void OnDisable()
    {
        controls.Player1.Disable();
        controls.Player1.Jump.performed -= _ => Jump();
    }
    private void Update()
    {
        moveDirection = transform.forward; // forward direction of the player
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, HexTileLayer);
    }
    private void FixedUpdate()
    {
        moveInput = controls.Player1.Move.ReadValue<Vector2>(); // Get movement input from input system
        MovePlayer();
        float currentSpeed = _rigidbody.linearVelocity.magnitude;
    }

    private void MovePlayer()
    {
        // Move relative to camera orientation
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Make sure y is 0 for up and down and normalize vectors
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (right * moveInput.x + forward * moveInput.y) * moveSpeed; // Calculate movement vector
        _rigidbody.MovePosition(_rigidbody.position + movement * Time.deltaTime); // Move the rigidbody

        if (moveInput.magnitude > 0.001f)
        {
            moveDirection = movement.normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection); // Rotate player to face movement direction
        }
    }
    private void Jump()
    {
        if (!isJumping)
        {
            controls.Player1.Move.Disable();
            StartCoroutine(jumpSequence());
        }
    }
    private IEnumerator jumpSequence()
    {
        isJumping = true;
        Vector3 startPos = transform.position;

        // Jump consists out of 3 segments. Calculate the three key positions for the triangular jump
        Vector3 peakStartPos = startPos + (moveDirection * jumpDistance / 3) + (Vector3.up * jumpHeight);
        Vector3 peakEndPos = startPos + (moveDirection * jumpDistance * 2 / 3) + (Vector3.up * jumpHeight);
        Vector3 endPos = startPos + (moveDirection * jumpDistance);

        float elapsedTime = 0f;
        Vector3[] points = { startPos, peakStartPos, peakEndPos, endPos };

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;

            // Calculate the current segment (0-2)
            int segment = Mathf.Min(2, (int)(t * 3));
            float segmentT = (t * 3) - segment;

            // Linear interpolation between current segment points
            Vector3 currentPosition = Vector3.Lerp(points[segment], points[segment + 1], segmentT);

            transform.position = currentPosition;
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

            transform.position = endPos;
            isJumping = false;
            controls.Player1.Move.Enable();
        }
}