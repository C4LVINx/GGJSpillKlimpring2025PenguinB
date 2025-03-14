using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections; // For using coroutines

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
    public AudioClip notEnoughFundsSFX;  // Sound effect for insufficient funds
    public AudioClip purchaseSFX;        // Sound effect for purchase

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

            // Disable MusicManager temporarily to focus on the purchase sound
            if (musicManager != null && musicManager.audioSource != null)
            {
                musicManager.audioSource.Pause();  // Pause the music temporarily
            }

            // Play purchase sound effect
            if (audioSource != null && purchaseSFX != null)
            {
                audioSource.PlayOneShot(purchaseSFX);
            }

            // Wait for the purchase sound to finish, then load the next scene
            StartCoroutine(WaitForSFXAndLoadNextScene(purchaseSFX.length));  // Wait for the duration of the purchase SFX
        }
        else
        {
            Debug.Log("Not enough Yuzu Coins!");

            // Play the insufficient funds sound effect
            if (audioSource != null && notEnoughFundsSFX != null)
            {
                audioSource.PlayOneShot(notEnoughFundsSFX);
            }

            // Wait for the insufficient funds sound to finish, then load the next scene
            StartCoroutine(WaitForSFXAndLoadNextScene(notEnoughFundsSFX.length));  // Wait for the duration of the SFX
        }
    }

    // Coroutine that waits for the SFX to finish, then loads the next scene
    private IEnumerator WaitForSFXAndLoadNextScene(float sfxDuration)
    {
        // Wait for the SFX to finish playing
        yield return new WaitForSeconds(sfxDuration);

        // Load the next scene after the SFX finishes
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load! This is the last scene.");
        }
    }
    private IEnumerator WaitAndLoadNextScene(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        // Load the next scene after the delay
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load! This is the last scene.");
        }
    }
}