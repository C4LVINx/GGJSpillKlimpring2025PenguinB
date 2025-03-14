using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    [Header("Catch Settings")]
    public float catchRadius = 5f; // Radius to catch an object
    public string interactableTag = "TrappedObject"; // Tag for the trapped objects
    private PlayerInput playerInput;  // Reference to the player input
    private StorageSystem storageSystem;  // Reference to the storage system

    [Header("Sound Effects")]
    public AudioClip catchSFX;  // Sound effect for catching an object
    private AudioSource audioSource;  // AudioSource to play the catch sound effect

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        storageSystem = FindObjectOfType<StorageSystem>(); // Get the storage system in the scene
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component

        // Check if the AudioSource component is attached
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the Catch input action (catch objects when this is pressed)
        playerInput.actions["Catch"].performed += OnCatch;
    }

    private void OnDisable()
    {
        // Unsubscribe from the Catch input action
        playerInput.actions["Catch"].performed -= OnCatch;
    }

    private void OnCatch(InputAction.CallbackContext context)
    {
        // Find all trapped objects within the catch radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, catchRadius);

        foreach (var collider in colliders)
        {
            // If the object is tagged as a trapped object, store it
            if (collider.CompareTag(interactableTag))
            {
                Debug.Log($"Caught {collider.gameObject.name}");

                // Play the catch sound effect (if assigned)
                if (catchSFX != null && audioSource != null)
                {
                    audioSource.PlayOneShot(catchSFX);  // Play the sound effect
                }
                else
                {
                    Debug.LogError("Catch sound effect or AudioSource is not assigned correctly.");
                }

                // Store the object in the storage system
                storageSystem.StoreCaughtObject(collider.gameObject);

                // Destroy the object after catching (it is now stored in the inventory)
                Destroy(collider.gameObject);
            }
        }
    }
}