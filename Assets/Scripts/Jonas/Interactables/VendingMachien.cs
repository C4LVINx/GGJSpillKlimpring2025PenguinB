using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VendingMachine : MonoBehaviour
{
    [Header("Vending Machine UI")]
    public GameObject vendingUI;
    public Button buyButton;
    public Button exitButton;
    public Text priceText;
    public Text yuzuCoinsText;
    public int price = 10;

    private PlayerShooting playerShoot;
    private PlayerMove playerMove;
    private StorageSystem storageSystem;
    private GameObject playerUI;
    private MusicManager musicManager;
    private bool isInteracting = false;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip notEnoughFundsSFX; // Sound effect for insufficient funds

    private void Awake()
    {
        storageSystem = FindObjectOfType<StorageSystem>();
        playerShoot = FindObjectOfType<PlayerShooting>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerUI = GameObject.Find("PlayerUI");
        musicManager = FindObjectOfType<MusicManager>();

        if (vendingUI != null) vendingUI.SetActive(false);
        if (buyButton != null) buyButton.onClick.AddListener(OnBuyItem);
        if (exitButton != null) exitButton.onClick.AddListener(CloseVendingUI);

        if (priceText != null)
            priceText.text = "Price: " + price + " Yuzu Coins";
    }

    private void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            OpenVendingUI();
        }

        if (vendingUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseVendingUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteracting = false;
            CloseVendingUI();
        }
    }

    public void OpenVendingUI()
    {
        if (vendingUI != null) vendingUI.SetActive(true);
        if (yuzuCoinsText != null) yuzuCoinsText.text = "Current Yuzu Coins: " + storageSystem.yuzuCoins;

        if (playerShoot != null) playerShoot.enabled = false;
        if (playerMove != null) playerMove.SetPause(true);
        if (playerUI != null) playerUI.SetActive(false);

        if (musicManager != null) musicManager.PlayVendingMusic();
    }

    public void CloseVendingUI()
    {
        if (vendingUI != null) vendingUI.SetActive(false);

        if (playerShoot != null) playerShoot.enabled = true;
        if (playerMove != null) playerMove.SetPause(false);
        if (playerUI != null) playerUI.SetActive(true);

        if (musicManager != null) musicManager.StopSpecialMusic();
    }

    private void OnBuyItem()
    {
        if (storageSystem.yuzuCoins >= price)
        {
            storageSystem.SpendYuzuCoins(price);
            Debug.Log("Item purchased successfully!");
            CloseVendingUI();
        }
        else
        {
            Debug.Log("Not enough Yuzu Coins!");

            // 🎵 Play the insufficient funds sound effect
            if (audioSource != null && notEnoughFundsSFX != null)
            {
                audioSource.PlayOneShot(notEnoughFundsSFX);
            }
        }
    }
}
