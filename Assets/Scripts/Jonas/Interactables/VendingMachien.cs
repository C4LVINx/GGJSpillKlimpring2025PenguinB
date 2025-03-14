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
    private MusicManager musicManager; // Reference to the MusicManager for music control
    private bool isInteracting = false; // To check if the player is interacting with the vending machine
    private bool isVendingOpen = false; // Track if the vending machine UI is open

    private void Awake()
    {
        storageSystem = FindObjectOfType<StorageSystem>(); // Find the storage system in the scene
        playerShoot = FindObjectOfType<PlayerShooting>(); // Find the player shooting script
        playerMove = FindObjectOfType<PlayerMove>(); // Find the player movement script
        playerUI = GameObject.Find("PlayerUI"); // Find the player's UI (assuming it's named "PlayerUI")
        musicManager = FindObjectOfType<MusicManager>(); // Find the MusicManager in the scene

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
    public void OpenVendingUI()
    {
        if (!isVendingOpen) // Only open if it's not already open
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

            // Switch to vending music
            if (musicManager != null)
            {
                musicManager.PlayMusic(musicManager.vendingMusic); // Play vending music
            }

            isVendingOpen = true; // Mark the vending UI as open
        }
    }

    // Close the vending machine UI
    public void CloseVendingUI()
    {
        if (isVendingOpen) // Only close if it's open
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

            // Switch back to default music
            if (musicManager != null)
            {
                musicManager.PlayMusic(musicManager.defaultMusic); // Play default music
            }

            isVendingOpen = false; // Mark the vending UI as closed
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

            // Close the vending UI after purchase
            CloseVendingUI();
        }
        else
        {
            Debug.Log("Not enough Yuzu Coins!");
        }
    }
}