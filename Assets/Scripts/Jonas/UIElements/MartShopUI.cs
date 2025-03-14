using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MartShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject shopPanel; // The UI panel for the Capymart shop
    public Text storedObjectsText; // Display stored objects count
    public Button closeShopButton; // Button to close the shop
    public Button purchaseYuzuCoinButton; // Button to purchase Yuzu Coin

    [Header("Purchase Settings")]
    public int yuzuCoinCost = 1; // The cost of 1 Yuzu Coin in Stored Objects

    private StorageSystem storageSystem; // Reference to the StorageSystem
    private PlayerInput playerInput; // Reference to the player's input
    private PlayerUI playerUI; // Reference to the PlayerUI script
    private PlayerShooting playerShoot; // Reference to the PlayerShooting script
    private MusicManager musicManager; // Reference to the MusicManager script

    private bool isShopOpen = false; // Track if the shop is currently open

    private void Start()
    {
        storageSystem = FindObjectOfType<StorageSystem>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerUI = FindObjectOfType<PlayerUI>(); // Get the PlayerUI script
        playerShoot = FindObjectOfType<PlayerShooting>(); // Get the PlayerShooting script
        musicManager = FindObjectOfType<MusicManager>(); // Get the MusicManager script

        if (storageSystem == null || playerInput == null || playerUI == null || playerShoot == null || musicManager == null)
        {
            Debug.LogError("Necessary components missing!");
            return;
        }

        // Link the button to the method for purchasing Yuzu Coin
        if (purchaseYuzuCoinButton != null)
        {
            purchaseYuzuCoinButton.onClick.AddListener(PurchaseYuzuCoin);
        }

        // Set up the close button
        if (closeShopButton != null)
        {
            closeShopButton.onClick.AddListener(CloseShop);
        }

        shopPanel.SetActive(false); // Hide the shop initially
    }

    private void Update()
    {
        storedObjectsText.text = "Insects: " + storageSystem.storedObjects.Count;
    }

    // Open the Capymart shop
    public void OpenShop()
    {
        if (!isShopOpen) // Only open the shop if it's not already open
        {
            shopPanel.SetActive(true); // Show the shop
            Time.timeScale = 0; // Pause game time
            playerInput.enabled = false; // Disable player input

            if (playerShoot != null)
            {
                playerShoot.enabled = false; // Disable shooting while in the shop
            }

            if (playerUI != null)
            {
                playerUI.gameObject.SetActive(false); // Disable Player UI when interacting with the shop
            }

            // Switch to shop music
            if (musicManager != null)
            {
                musicManager.PlayMusic(musicManager.shopMusic);
            }

            isShopOpen = true; // Mark shop as open
        }
    }

    // Close the Capymart shop
    public void CloseShop()
    {
        if (isShopOpen) // Only close the shop if it's open
        {
            shopPanel.SetActive(false); // Hide the shop
            Time.timeScale = 1; // Resume game time
            playerInput.enabled = true; // Re-enable player input

            if (playerShoot != null)
            {
                playerShoot.enabled = true; // Re-enable shooting when exiting the shop
            }

            if (playerUI != null)
            {
                playerUI.gameObject.SetActive(true); // Re-enable Player UI when exiting the shop
            }

            // Switch to default music
            if (musicManager != null)
            {
                musicManager.PlayMusic(musicManager.defaultMusic);
            }

            isShopOpen = false; // Mark shop as closed
        }
    }

    // Handle the Yuzu Coin purchase
    public void PurchaseYuzuCoin()
    {
        // Check if the player has enough stored objects to buy a Yuzu Coin
        if (storageSystem.storedObjects.Count >= yuzuCoinCost)
        {
            // Deduct the required stored objects
            for (int i = 0; i < yuzuCoinCost; i++)
            {
                if (storageSystem.storedObjects.Count > 0)
                {
                    storageSystem.storedObjects.RemoveAt(0); // Remove one stored object
                }
            }

            // Add 1 Yuzu Coin to the player's inventory
            storageSystem.AddYuzuCoins(1); // You can adjust this to match the cost of 1 Yuzu Coin

            Debug.Log("Purchased Yuzu Coin!");

            // Update the PlayerUI to reflect the new Yuzu coins count
            if (playerUI != null)
            {
                playerUI.ForceUpdateYuzuCoins(); // Update the UI immediately after purchase
            }
        }
        else
        {
            Debug.Log("Not enough stored objects to buy a Yuzu coin.");
        }
    }
}