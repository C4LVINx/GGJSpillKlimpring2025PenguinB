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

    [Header("Audio")]
    public AudioSource audioSource; // AudioSource for sound effects
    public AudioClip purchaseSound; // Sound for successful purchase
    public AudioClip insufficientFundsSound; // Sound when purchase fails

    private StorageSystem storageSystem; // Reference to the StorageSystem
    private PlayerInput playerInput; // Reference to PlayerInput
    private PlayerUI playerUI; // Reference to the PlayerUI script
    private PlayerShooting playerShoot; // Reference to the PlayerShooting script
    private MusicManager musicManager; // Reference to the MusicManager

    private bool isShopOpen = false; // Track if the shop is open

    private void Start()
    {
        storageSystem = FindObjectOfType<StorageSystem>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerUI = FindObjectOfType<PlayerUI>();
        playerShoot = FindObjectOfType<PlayerShooting>();
        musicManager = FindObjectOfType<MusicManager>(); // Get MusicManager reference

        if (storageSystem == null) Debug.LogError("StorageSystem not found!");
        if (playerInput == null) Debug.LogError("PlayerInput component is missing!");
        if (playerUI == null) Debug.LogError("PlayerUI script is missing!");
        if (playerShoot == null) Debug.LogError("PlayerShooting script is missing!");
        if (musicManager == null) Debug.LogError("MusicManager is missing! Make sure it's added to the scene.");

        if (purchaseYuzuCoinButton != null)
        {
            purchaseYuzuCoinButton.onClick.AddListener(PurchaseYuzuCoin);
        }

        if (closeShopButton != null)
        {
            closeShopButton.onClick.AddListener(CloseShop);
        }

        shopPanel.SetActive(false); // Ensure shop starts hidden
    }

    private void Update()
    {
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        if (isShopOpen) return;

        isShopOpen = true;
        shopPanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerShoot != null) playerShoot.enabled = false;
        if (playerUI != null) playerUI.gameObject.SetActive(false);
        if (playerInput != null) playerInput.enabled = false;

        if (musicManager != null)
        {
            musicManager.PlayShopMusic();
        }

        UpdateUI();
    }

    public void CloseShop()
    {
        if (!isShopOpen) return;

        isShopOpen = false;
        shopPanel.SetActive(false);
        Time.timeScale = 1f;

        if (playerShoot != null) playerShoot.enabled = true;
        if (playerUI != null) playerUI.gameObject.SetActive(true);
        if (playerInput != null) playerInput.enabled = true;

        if (musicManager != null)
        {
            musicManager.StopSpecialMusic();
        }
    }

    public void PurchaseYuzuCoin()
    {
        if (storageSystem == null) return;

        if (storageSystem.storedObjects.Count >= yuzuCoinCost)
        {
            for (int i = 0; i < yuzuCoinCost; i++)
            {
                storageSystem.storedObjects.RemoveAt(0);
            }

            storageSystem.AddYuzuCoins(1);

            // Play purchase sound effect
            if (audioSource != null && purchaseSound != null)
            {
                audioSource.PlayOneShot(purchaseSound);
            }

            Debug.Log("Purchased Yuzu Coin!");
        }
        else
        {
            if (audioSource != null && insufficientFundsSound != null)
            {
                audioSource.PlayOneShot(insufficientFundsSound);
            }

            Debug.Log("Not enough insects to buy a Yuzu Coin.");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (storedObjectsText != null && storageSystem != null)
        {
            storedObjectsText.text = "Insects: " + storageSystem.storedObjects.Count;
        }
    }
}