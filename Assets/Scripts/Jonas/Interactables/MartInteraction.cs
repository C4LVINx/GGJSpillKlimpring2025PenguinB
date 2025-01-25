using UnityEngine;
using UnityEngine.InputSystem;

public class MartInteraction : MonoBehaviour
{
    [Header("Mart Settings")]
    public string martName = "Mart";  // Name of the mart
    public float interactionRadius = 3f; // Radius to interact with mart
    public StorageSystem storageSystem; // Reference to the storage system
    public Transform playerTransform;  // Reference to the player transform

    private PlayerInput playerInput;  // Player input reference

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
        }
    }

    private void Start()
    {
        // Assuming the player has a Camera that represents their transform
        if (playerTransform == null)
        {
            playerTransform = Camera.main?.transform; // This assumes the player's camera is the player's transform
            if (playerTransform == null)
            {
                Debug.LogError("Player transform not found! Ensure the playerTransform is set correctly.");
            }
        }
    }

    private void OnEnable()
    {
        // Subscribe to the Interact input action
        playerInput.actions["Interact"].performed += OnInteract;
    }

    private void OnDisable()
    {
        // Unsubscribe from the Interact input action
        playerInput.actions["Interact"].performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Debug the positions to see what they are
        if (playerTransform != null)
        {
            Debug.Log($"Mart Position: {transform.position}");
            Debug.Log($"Player Position: {playerTransform.position}");

            // Check if the player is within interaction range
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= interactionRadius)
            {
                OpenMartMenu();
            }
            else
            {
                Debug.Log("You are too far away from the mart to interact.");
            }
        }
        else
        {
            Debug.LogError("Player transform is not set!");
        }
    }

    private void OpenMartMenu()
    {
        Debug.Log($"Interacting with {martName}. Here are your options!");

        // Simulate interaction with the mart (you could show a UI here)
        // For now, we'll use the console for debugging
        if (storageSystem.storedObjects.Count > 0)
        {
            Debug.Log("Available for purchase:");
            // You can implement more specific mart items here
            Debug.Log("Item 1 - Buy using stored object.");
        }
        else
        {
            Debug.Log("You don't have any stored objects to buy items.");
        }
    }
}