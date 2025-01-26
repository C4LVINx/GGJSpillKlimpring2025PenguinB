using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    [Header("Catch Settings")]
    public float catchRadius = 5f; // Radius to catch an object
    public string interactableTag = "TrappedObject"; // Tag for the trapped objects
    private PlayerInput playerInput;  // Reference to the player input
    private StorageSystem storageSystem;  // Reference to the storage system

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        storageSystem = FindObjectOfType<StorageSystem>(); // Get the storage system in the scene
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

                // Store the object in the storage system
                storageSystem.StoreCaughtObject(collider.gameObject);

                // Destroy the object after catching (it is now stored in the inventory)
                Destroy(collider.gameObject);
            }
        }
    }
}
