using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerMovement _inputActions;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotSpeed = 720f;

    private Vector3 _moveDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputActions = new PlayerMovement(); // Initialize the input actions
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.InputPlayer.Move.performed += OnMovePerformed; // Use InputPlayer action map
        _inputActions.InputPlayer.Move.canceled += OnMoveCanceled;  // Handle canceled input
    }

    private void OnDisable()
    {
        _inputActions.InputPlayer.Move.performed -= OnMovePerformed;
        _inputActions.InputPlayer.Move.canceled -= OnMoveCanceled;
        _inputActions.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0, input.y).normalized; // Convert to 3D direction
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveDirection = Vector3.zero; // Stop movement when input is canceled
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector3.zero)
        {
            // Move the character
            Vector3 movement = _moveDirection * _moveSpeed * Time.deltaTime;
            _controller.Move(movement);

            // Rotate the character to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }
}

