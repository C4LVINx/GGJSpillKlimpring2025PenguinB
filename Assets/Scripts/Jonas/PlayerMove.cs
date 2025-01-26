using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerMovement _inputActions;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotSpeed = 720f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _groundedGravity = -0.1f;

    private Vector3 _moveDirection;
    private Vector3 _velocity;

    private bool isPaused = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputActions = new PlayerMovement();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.InputPlayer.Move.performed += OnMovePerformed;
        _inputActions.InputPlayer.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        _inputActions.InputPlayer.Move.performed -= OnMovePerformed;
        _inputActions.InputPlayer.Move.canceled -= OnMoveCanceled;
        _inputActions.Disable();
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isPaused) return;

        Vector2 input = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isPaused) return;

        _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (isPaused) return;

        if (_controller.isGrounded)
        {
            // Reset vertical velocity when grounded
            _velocity.y = _groundedGravity;
        }
        else
        {
            // Apply gravity when not grounded
            _velocity.y -= _gravity * Time.deltaTime;
        }

        // Calculate movement
        Vector3 movement = (_moveDirection * _moveSpeed + _velocity) * Time.deltaTime;

        // Move the character
        _controller.Move(movement);

        // Rotate the character to face movement direction
        if (_moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
        if (pause) _moveDirection = Vector3.zero;
    }
}