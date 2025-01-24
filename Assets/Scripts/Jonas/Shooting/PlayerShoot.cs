using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullets
    [SerializeField] private Transform bulletSpawnPoint; // Empty GameObject for bullet spawn location
    [SerializeField] private float bulletSpeed = 10f; // Speed of the bullets
    [SerializeField] private LayerMask aimLayerMask; // LayerMask to detect where the mouse points

    private PlayerMovement _inputActions;
    private Camera _mainCamera;

    private void Awake()
    {
        _inputActions = new PlayerMovement();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.InputPlayer.Shoot.performed += OnShootPerformed; // Hook into Shoot action
        _inputActions.InputPlayer.Aim.performed += OnAimPerformed;   // Hook into Aim action
    }

    private void OnDisable()
    {
        _inputActions.InputPlayer.Shoot.performed -= OnShootPerformed;
        _inputActions.InputPlayer.Aim.performed -= OnAimPerformed;
        _inputActions.Disable();
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        // Spawn and shoot the bullet
        ShootBullet();
    }

    private void OnAimPerformed(InputAction.CallbackContext context)
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = context.ReadValue<Vector2>();

        // Raycast from the mouse position to the game world
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayerMask))
        {
            // Move the bulletSpawnPoint to the hit position
            Vector3 aimDirection = hit.point - transform.position;
            aimDirection.y = 0; // Keep it on the same plane as the player

            // Rotate the spawn point to face the aim direction
            bulletSpawnPoint.position = transform.position + aimDirection.normalized;
            bulletSpawnPoint.rotation = Quaternion.LookRotation(aimDirection);
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        // Spawn the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Add velocity to the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Optionally, destroy the bullet after a certain time
        Destroy(bullet, 5f);
    }
}