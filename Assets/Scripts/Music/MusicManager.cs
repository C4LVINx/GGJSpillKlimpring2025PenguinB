using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Tracks")]
    public AudioClip defaultMusic; // Default background music
    public AudioClip shopMusic;    // Music when the shop is open
    public AudioClip vendingMusic; // Music when the vending machine is open

    private AudioSource audioSource; // The audio source to play music
    private AudioClip currentMusic;  // To track which music is currently playing

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
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

    // Function to play a specific music track
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource != null && currentMusic != clip) // Check if the music is already playing
        {
            currentMusic = clip; // Update the current music
            audioSource.Stop(); // Stop the current music
            audioSource.clip = clip; // Set the new music clip
            audioSource.Play(); // Play the new music
        }
    }

    // Optionally you can also have functions to stop or fade the music, but this is the basic setup
    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}