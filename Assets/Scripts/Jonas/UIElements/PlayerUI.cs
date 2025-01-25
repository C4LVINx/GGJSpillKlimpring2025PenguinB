using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text storedObjectsText; // Text to display stored objects count
    public Text yuzuCoinsText; // Text to display Yuzu coins count

    private StorageSystem storageSystem; // Reference to the StorageSystem

    private void Start()
    {
        storageSystem = FindObjectOfType<StorageSystem>(); // Get the storage system in the scene
        if (storageSystem == null)
        {
            Debug.LogError("StorageSystem not found in the scene!");
        }
    }

    private void Update()
    {
        // Update the stored objects text in real-time
        if (storedObjectsText != null && storageSystem != null)
        {
            storedObjectsText.text = "Stored Objects: " + storageSystem.storedObjects.Count;
        }

        // Update the Yuzu coins text in real-time
        if (yuzuCoinsText != null && storageSystem != null)
        {
            yuzuCoinsText.text = "Yuzu Coins: " + storageSystem.yuzuCoins;
        }
    }

    // Add a method to force an immediate UI update for Yuzu Coins
    public void ForceUpdateYuzuCoins()
    {
        if (yuzuCoinsText != null && storageSystem != null)
        {
            yuzuCoinsText.text = "Yuzu Coins: " + storageSystem.yuzuCoins;
        }
        else
        {
            Debug.LogError("YuzuCoinsText is not assigned in PlayerUI.");
        }
    }
}