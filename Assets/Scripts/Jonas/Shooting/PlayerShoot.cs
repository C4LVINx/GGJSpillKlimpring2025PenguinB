using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // The prefab for the bullet to spawn
    public Transform bulletSpawnPoint; // The point where bullets are instantiated
    public float bulletSpeed = 20f; // Speed of the bullet

    [Header("Input")]
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Subscribe to the input event for shooting
        playerInput.actions["Shoot"].performed += OnShoot;
    }

    private void OnDisable()
    {
        // Unsubscribe from the input event for shooting
        playerInput.actions["Shoot"].performed -= OnShoot;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // Ensure the action was performed (not canceled, etc.)

        Debug.Log("Shooting!");

        // Spawn the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Add velocity to the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Optional: Destroy the bullet after a certain time to avoid clutter
        Destroy(bullet, 5f);
    }
}