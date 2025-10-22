using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Rigidbody _rigidbody;

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

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Jump.performed += _ => Jump();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
        controls.Player.Jump.performed -= _ => Jump();
    }
    private void Update()
    {
        moveDirection = transform.forward;
    }
    private void FixedUpdate()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        MovePlayer();
        float currentSpeed = _rigidbody.linearVelocity.magnitude;
    }

    private void MovePlayer()
    {
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (right * moveInput.x + forward * moveInput.y) * moveSpeed;
        _rigidbody.MovePosition(_rigidbody.position + movement * Time.deltaTime);

        if (moveInput.magnitude > 0.001f)
        {
            moveDirection = movement.normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
    private void Jump()
    {
        if (!isJumping)
        {
            controls.Player.Move.Disable();
            StartCoroutine(jumpSequence());
        }
    }
    private IEnumerator jumpSequence()
    {
        isJumping = true;
        Vector3 startPos = transform.position;

        // Calculate the three key positions for the triangular jump
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
            controls.Player.Move.Enable();
        }
}