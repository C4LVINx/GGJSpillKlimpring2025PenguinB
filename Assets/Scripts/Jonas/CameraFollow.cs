using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // The player to follow

    [Header("Camera Offset and Angle")]
    public Vector3 offset = new Vector3(0, 10, -10); // Offset from the player
    public Vector3 rotationAngles = new Vector3(45, 45, 0); // Rotation for isometric view (X, Y, Z angles)

    [Header("Camera Movement")]
    public float followSpeed = 5f; // Speed at which the camera follows the player

    private void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position
            Vector3 desiredPosition = player.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Apply the rotation angles from the Inspector
            transform.rotation = Quaternion.Euler(rotationAngles);
        }
    }
}