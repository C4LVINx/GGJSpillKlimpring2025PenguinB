using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint; // Empty GameObject for bullet spawning
    [SerializeField] private float radius = 2f; // Distance from the player
    [SerializeField] private LayerMask aimLayerMask; // Layers to aim at

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main; // Get the main camera
    }

    private void Update()
    {
        // Get the mouse position from the screen
        Vector2 mousePosition = Input.mousePosition;

        // Raycast from the mouse position into the game world
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayerMask))
        {
            // Calculate the direction from the player to the mouse pointer
            Vector3 direction = hit.point - transform.position;
            direction.y = 0; // Keep it on the same horizontal plane

            // Normalize the direction vector and scale it by the radius
            Vector3 clampedPosition = transform.position + direction.normalized * radius;

            // Move the bullet spawn point to the calculated position
            bulletSpawnPoint.position = clampedPosition;

            // Rotate the bullet spawn point to face the mouse pointer
            bulletSpawnPoint.LookAt(hit.point);
        }
    }
}