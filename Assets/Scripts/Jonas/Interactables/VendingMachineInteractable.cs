using UnityEngine;
using UnityEngine.InputSystem;

public class VendingMachineInteractble : MonoBehaviour
{
    [Header("Vending Machine Settings")]
    public string vendingMachineName = "Vending Machine";  // Name of the vending machine
    public float interactionRadius = 3f; // Radius to interact with vending machine
    public StorageSystem storageSystem; // Reference to the storage system
    public Transform playerTransform;  // Reference to the player transform

    private PlayerInput playerInput;  // Player input reference
    private VendingMachine vendingMachine;  // Reference to the VendingMachine script

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
        }

        // Get the VendingMachine component
        vendingMachine = GetComponent<VendingMachine>();
        if (vendingMachine == null)
        {
            Debug.LogError("VendingMachine component not found!");
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
            Debug.Log($"Vending Machine Position: {transform.position}");
            Debug.Log($"Player Position: {playerTransform.position}");

            // Check if the player is within interaction range
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= interactionRadius)
            {
                OpenVendingMachineUI();
            }
            else
            {
                Debug.Log("You are too far away from the vending machine to interact.");
            }
        }
        else
        {
            Debug.LogError("Player transform is not set!");
        }
    }

    private void OpenVendingMachineUI()
    {
        // Show the vending machine UI when the player interacts with it
        if (vendingMachine != null)
        {
            vendingMachine.OpenVendingUI();  // Calls the OpenVendingUI() method in the VendingMachine script
        }
        else
        {
            Debug.LogError("VendingMachine component not found!");
        }
    }
}
