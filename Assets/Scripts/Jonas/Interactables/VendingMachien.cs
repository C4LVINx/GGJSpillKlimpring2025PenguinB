using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VendingMachine : MonoBehaviour
{
    [Header("Vending Machine UI")]
    public GameObject vendingUI; // The UI that shows when interacting with the vending machine
    public Button buyButton; // Button to buy the item
    public Button exitButton; // Exit button to close the vending machine UI
    public Text priceText; // Text to display the price
    public Text yuzuCoinsText; // Text to display the current Yuzu Coins
    public int price = 10; // Price for an item in Yuzu coins

    private PlayerShooting playerShoot; // Reference to the player shooting script
    private PlayerMove playerMove; // Reference to the player movement script
    private StorageSystem storageSystem; // Reference to the storage system
    private GameObject playerUI; // Reference to the player's UI (HUD, etc.)
    private bool isInteracting = false; // To check if the player is interacting with the vending machine

    private void Awake()
    {
        storageSystem = FindObjectOfType<StorageSystem>(); // Find the storage system in the scene
        playerShoot = FindObjectOfType<PlayerShooting>(); // Find the player shooting script
        playerMove = FindObjectOfType<PlayerMove>(); // Find the player movement script
        playerUI = GameObject.Find("PlayerUI"); // Find the player's UI (assuming it's named "PlayerUI")

        // Hide the vending UI initially
        if (vendingUI != null)
        {
            vendingUI.SetActive(false);
        }

        // Add listener to the buy button
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyItem);
        }

        // Add listener to the exit button (to close the vending machine UI)
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(CloseVendingUI);
        }

        // Update the price text
        if (priceText != null)
        {
            priceText.text = "Price: " + price + " Yuzu Coins";
        }
    }

    private void Update()
    {
        // Check for interaction with the vending machine
        if (isInteracting && Input.GetKeyDown(KeyCode.E)) // Press E to interact
        {
            OpenVendingUI();
        }

        // Allow closing the UI with 'Escape' key or assigned exit button
        if (vendingUI.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q)))
        {
            CloseVendingUI(); // Close UI with 'Escape' or 'Q' key
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = true; // Player is in range of the vending machine
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = false; // Player left the range
            CloseVendingUI(); // Close the UI if the player moves away
        }
    }

    // Open the vending machine UI
    public void OpenVendingUI() // Changed to public
    {
        if (vendingUI != null)
        {
            vendingUI.SetActive(true);
        }

        // Update the Yuzu Coins text in the vending machine UI
        if (yuzuCoinsText != null && storageSystem != null)
        {
            yuzuCoinsText.text = "Current Yuzu Coins: " + storageSystem.yuzuCoins;
        }

        // Disable player shooting and movement while interacting with the vending machine
        if (playerShoot != null)
        {
            playerShoot.enabled = false;
        }

        if (playerMove != null)
        {
            playerMove.SetPause(true); // Pause movement while interacting with the vending machine
        }

        // Disable player UI (e.g., HUD) while interacting with vending machine
        if (playerUI != null)
        {
            playerUI.SetActive(false); // Disable the player's UI
        }
    }

    // Close the vending machine UI
    public void CloseVendingUI() // Changed to public
    {
        if (vendingUI != null)
        {
            vendingUI.SetActive(false);
        }

        // Enable player shooting and movement when UI is closed
        if (playerShoot != null)
        {
            playerShoot.enabled = true;
        }

        if (playerMove != null)
        {
            playerMove.SetPause(false); // Unpause movement when closing the UI
        }

        // Enable player UI (e.g., HUD) when UI is closed
        if (playerUI != null)
        {
            playerUI.SetActive(true); // Enable the player's UI
        }
    }

    // Handle item purchase
    private void OnBuyItem()
    {
        // Check if the player has enough Yuzu Coins to buy the item
        if (storageSystem.yuzuCoins >= price)
        {
            // Deduct Yuzu Coins from the player
            storageSystem.SpendYuzuCoins(price);

            Debug.Log("Item purchased successfully!");

            // Add logic to give the item to the player (e.g., Boba drink)
            // For example, instantiate the item or play an animation.

            // Transition to the next scene after purchase
            LoadNextScene();

            CloseVendingUI(); // Close the vending machine UI after purchase
        }
        else
        {
            Debug.Log("Not enough Yuzu Coins!");
        }
    }

    // Load the next scene
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Get the current scene index
        int nextSceneIndex = currentSceneIndex + 1; // Calculate the next scene index

        // Check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // Load the next scene
        }
        else
        {
            Debug.LogWarning("No more scenes to load! This is the last scene.");
        }
    }
}