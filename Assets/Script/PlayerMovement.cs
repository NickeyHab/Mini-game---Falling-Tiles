using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerInput playerInput;
    private InputSystem_Actions controls;
    private Rigidbody _rigidbody;
    private Vector2 moveInput;
    private Camera playerCamera;

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
    }

    private void OnDisable()
    {
        controls.Player.Disable();
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
    }
}
