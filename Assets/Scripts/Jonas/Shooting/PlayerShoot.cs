using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // The prefab for the bullet to spawn
    public Transform bulletSpawnPoint; // The point where bullets are instantiated
    public float bulletSpeed = 20f; // Speed of the bullet
    public float bulletSpread = 0.05f; // Amount of spread for bullets

    [Header("Bubble Settings")]
    public GameObject bubblePrefab; // The prefab for the bubble effect
    public float bubbleDuration = 5f; // How long the bubble lasts

    [Header("Input")]
    private PlayerInput playerInput;
    private AudioSource audioSource;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
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

        // Play shooting sound
        if (audioSource != null && audioSource.isActiveAndEnabled)
        {
            audioSource.Play();
        }

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

        // Add collision detection to the bullet
        BulletCollision bulletCollision = bullet.AddComponent<BulletCollision>();
        bulletCollision.bubblePrefab = bubblePrefab;
        bulletCollision.bubbleDuration = bubbleDuration;
    }
}

public class BulletCollision : MonoBehaviour
{
    [Header("Bubble Settings")]
    public GameObject bubblePrefab; // The prefab for the bubble effect
    public float bubbleDuration = 5f; // How long the bubble lasts

    private void OnCollisionEnter(Collision collision)
    {
        // Debugging collision
        Debug.Log($"Bullet collided with: {collision.gameObject.name}");

        // If the bullet collides with an object tagged as TrapTarget
        if (collision.gameObject.CompareTag("TrapTarget"))
        {
            Debug.Log($"Trapping {collision.gameObject.name} in a bubble");
            TrapInBubble(collision.gameObject);
        }
        else
        {
            Debug.Log("Bullet did not hit a TrapTarget.");
        }
    }

    private void TrapInBubble(GameObject target)
    {
        // Change the tag of the object to "TrappedObject"
        target.tag = "TrappedObject";
        Debug.Log($"Changed {target.name}'s tag to TrappedObject");

        // Check if the target has a Rigidbody
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            // Debugging Rigidbody state
            Debug.Log($"Target {target.name} Rigidbody found. Freezing it.");
            targetRb.isKinematic = true; // Freeze the target's physics
        }
        else
        {
            Debug.Log($"No Rigidbody found on {target.name}");
        }

        // Create a bubble around the object
        GameObject bubble = Instantiate(bubblePrefab, target.transform.position, Quaternion.identity);
        bubble.transform.SetParent(target.transform); // Make the bubble follow the target

        // Optionally, apply some scale or effect to the bubble to make it look more fun
        bubble.transform.localScale = new Vector3(2f, 2f, 2f); // Adjust as needed

        // Call the insect's TrapInBubble method to stop its movement
        InsectMovement insectMovement = target.GetComponent<InsectMovement>();
        if (insectMovement != null)
        {
            insectMovement.TrapInBubble();
        }

        // Destroy the bubble after the duration
        Destroy(bubble, bubbleDuration);

        // Optionally, unfreeze the object after the bubble expires
        StartCoroutine(UnfreezeAfterBubble(target, bubbleDuration));
    }

    private System.Collections.IEnumerator UnfreezeAfterBubble(GameObject target, float duration)
    {
        // Wait for the duration of the bubble
        yield return new WaitForSeconds(duration);

        // Unfreeze the object and restore its movement
        if (target != null)
        {
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                targetRb.isKinematic = false; // Re-enable physics on the trapped object
                Debug.Log($"Unfreezing {target.name} after bubble expires.");
            }

            // Change the tag back to "TrapTarget" once the bubble breaks
            target.tag = "TrapTarget";
            Debug.Log($"Changed {target.name}'s tag back to TrapTarget");

            // Call the insect's UntrapFromBubble method to resume its movement
            InsectMovement insectMovement = target.GetComponent<InsectMovement>();
            if (insectMovement != null)
            {
                insectMovement.UntrapFromBubble();
            }
        }
        else
        {
            Debug.Log("Target has already been destroyed, skipping unfreeze.");
        }
    }
}