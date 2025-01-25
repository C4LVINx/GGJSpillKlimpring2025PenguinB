using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MartShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject shopPanel; // The UI panel for the shop
    public Text storedObjectsText; // Text to display stored objects count
    public Button closeShopButton; // Button to close the shop

    [Header("Shop Items")]
    public ShopItem[] shopItems; // Array of items available in the shop

    private StorageSystem storageSystem; // Reference to the StorageSystem
    private PlayerInput playerInput; // Reference to the player's input
    private PlayerMove playerMove; // Reference to the player's movement script

    private void Start()
    {
        // Find required components in the scene
        storageSystem = FindObjectOfType<StorageSystem>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerMove = FindObjectOfType<PlayerMove>();

        if (storageSystem == null)
        {
            Debug.LogError("StorageSystem not found in the scene!");
            return;
        }

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found in the scene!");
            return;
        }

        if (playerMove == null)
        {
            Debug.LogError("PlayerMove script not found on the player!");
            return;
        }

        // Initialize shop item buttons
        foreach (var item in shopItems)
        {
            item.purchaseButton.onClick.AddListener(() => OnPurchaseItem(item));
        }

        // Set up close button
        if (closeShopButton != null)
        {
            closeShopButton.onClick.AddListener(CloseShop);
        }

        // Hide shop initially
        shopPanel.SetActive(false);
    }

    private void Update()
    {
        // Update the stored objects text in real-time
        UpdateStoredObjectsText();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0; // Pause game time
        PausePlayer(true); // Pause player movement and input
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1; // Resume game time
        PausePlayer(false); // Resume player movement and input
    }

    private void PausePlayer(bool isPaused)
    {
        if (playerInput != null)
        {
            playerInput.enabled = !isPaused; // Enable/disable player input
        }

        if (playerMove != null)
        {
            playerMove.SetPause(isPaused); // Pause movement logic in PlayerMove
        }
    }

    private void UpdateStoredObjectsText()
    {
        // Update the UI text to reflect the number of stored objects
        if (storedObjectsText != null)
        {
            storedObjectsText.text = "Stored Objects: " + storageSystem.storedObjects.Count;
        }
    }

    private void OnPurchaseItem(ShopItem item)
    {
        // Check if the player has enough stored objects to buy this item
        if (storageSystem.storedObjects.Count >= item.cost)
        {
            // Deduct the cost from the player's storage
            for (int i = 0; i < item.cost; i++)
            {
                if (storageSystem.storedObjects.Count > 0)
                {
                    storageSystem.storedObjects.RemoveAt(0);
                }
            }

            Debug.Log($"Purchased {item.itemName} for {item.cost} stored objects!");

            // Implement any additional logic for granting the item to the player
        }
        else
        {
            Debug.Log($"Not enough stored objects to purchase {item.itemName} (Cost: {item.cost}).");
        }

        // Update the UI after a purchase
        UpdateStoredObjectsText();
    }
}

[System.Serializable]
public class ShopItem
{
    public string itemName; // Name of the item
    public int cost; // Cost in stored objects
    public Button purchaseButton; // The button to purchase this item
}