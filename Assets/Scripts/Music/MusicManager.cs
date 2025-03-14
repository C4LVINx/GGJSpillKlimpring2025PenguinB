using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Tracks")]
    public AudioClip defaultMusic; // Default background music
    public AudioClip shopMusic;    // Music when the shop is open
    public AudioClip vendingMusic; // Music when the vending machine is open

    public AudioSource audioSource;
    private AudioClip currentMusic;

    private bool isShopOpen = false;
    private bool isVendingOpen = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject!");
        }
    }

    private void Start()
    {
        if (audioSource != null && defaultMusic != null)
        {
            PlayMusic(defaultMusic); // Start with default music
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource != null && currentMusic != clip)
        {
            currentMusic = clip;
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayShopMusic()
    {
        if (!isShopOpen) // Prevent restarting if already playing
        {
            PlayMusic(shopMusic);
            isShopOpen = true;
            isVendingOpen = false; // Ensure only one special track plays at a time
        }
    }

    public void PlayVendingMusic()
    {
        if (!isVendingOpen)
        {
            PlayMusic(vendingMusic);
            isVendingOpen = true;
            isShopOpen = false;
        }
    }

    public void PlayDefaultMusic()
    {
        if (isShopOpen || isVendingOpen) return; // Don't override if a menu is open
        PlayMusic(defaultMusic);
    }

    public void StopSpecialMusic() // Call when closing the shop or vending machine
    {
        isShopOpen = false;
        isVendingOpen = false;
        PlayDefaultMusic();
    }
}