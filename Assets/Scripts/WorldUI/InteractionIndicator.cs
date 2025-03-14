using UnityEngine;
using TMPro; // If using TextMeshPro

public class InteractionIndicator : MonoBehaviour
{
    public GameObject indicatorUI; // Assign the UI Canvas in Inspector

    private void Start()
    {
        indicatorUI.SetActive(false); // Hide on start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your Player has the "Player" tag
        {
            indicatorUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicatorUI.SetActive(false);
        }
    }
}
