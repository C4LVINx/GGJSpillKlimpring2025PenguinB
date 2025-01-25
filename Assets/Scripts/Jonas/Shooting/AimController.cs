using UnityEngine;

public class AimController : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // The player's transform

    [Header("Aim Settings")]
    public float radius = 2f; // Distance from the player
    public float rotationSpeed = 5f; // Speed of smooth rotation

    private Camera mainCamera; // The main camera in the scene

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned!");
            return;
        }

        // Get the mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Assume a flat ground plane
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(enter);

            // Calculate the direction from the player to the mouse position
            Vector3 direction = (mouseWorldPosition - player.position).normalized;

            // Position the empty GameObject in a circle around the player
            Vector3 targetPosition = player.position + direction * radius;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);

            // Make the empty GameObject face the mouse direction
            transform.LookAt(mouseWorldPosition);
        }
    }
}