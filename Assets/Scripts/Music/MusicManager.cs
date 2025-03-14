using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; // Singleton instance
    public AudioSource audioSource; // AudioSource to play music
    public AudioClip defaultMusic; // Background music for the game
    public AudioClip shopMusic; // Music for the shop

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of MusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the MusicManager between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        PlayMusic(defaultMusic); // Start with the default background music
    }

    // Function to play the given music clip
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting the same music clip
        audioSource.clip = clip; // Set the new music clip
        audioSource.Play(); // Play the music clip
        Debug.Log("Playing: " + clip.name); // Debug log to verify the current music
    }
}