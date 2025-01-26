using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Text Elements")]
    public TextMesh storedObjectsText; // 2D text for stored objects count
    public TextMesh yuzuCoinsText; // 2D text for Yuzu coins count

    [Header("Sprite Elements")]
    public SpriteRenderer storedObjectsSprite; // Sprite to represent stored objects
    public SpriteRenderer yuzuCoinsSprite; // Sprite to represent Yuzu coins

    private StorageSystem storageSystem; // Reference to the StorageSystem

    private void Start()
    {
        storageSystem = FindObjectOfType<StorageSystem>(); // Get the storage system in the scene
        if (storageSystem == null)
        {
            Debug.LogError("StorageSystem not found in the scene!");
        }

        // Ensure sprites are assigned
        if (storedObjectsSprite == null)
        {
            Debug.LogError("StoredObjectsSprite is not assigned in PlayerUI.");
        }
        if (yuzuCoinsSprite == null)
        {
            Debug.LogError("YuzuCoinsSprite is not assigned in PlayerUI.");
        }
    }

    private void Update()
    {
        // Update the stored objects text and sprite visibility in real-time
        if (storedObjectsText != null && storageSystem != null)
        {
            storedObjectsText.text = "Stored Objects: " + storageSystem.storedObjects.Count;

            // Update sprite visibility
            if (storedObjectsSprite != null)
            {
                storedObjectsSprite.enabled = storageSystem.storedObjects.Count > 0;
            }
        }

        // Update the Yuzu coins text and sprite visibility in real-time
        if (yuzuCoinsText != null && storageSystem != null)
        {
            yuzuCoinsText.text = "Yuzu Coins: " + storageSystem.yuzuCoins;

            // Update sprite visibility
            if (yuzuCoinsSprite != null)
            {
                yuzuCoinsSprite.enabled = storageSystem.yuzuCoins > 0;
            }
        }
    }

    // Add a method to force an immediate update for Yuzu Coins
    public void ForceUpdateYuzuCoins()
    {
        if (yuzuCoinsText != null && storageSystem != null)
        {
            yuzuCoinsText.text = "Yuzu Coins: " + storageSystem.yuzuCoins;

            // Update sprite visibility
            if (yuzuCoinsSprite != null)
            {
                yuzuCoinsSprite.enabled = storageSystem.yuzuCoins > 0;
            }
        }
        else
        {
            Debug.LogError("YuzuCoinsText or YuzuCoinsSprite is not assigned in PlayerUI.");
        }
    }
}