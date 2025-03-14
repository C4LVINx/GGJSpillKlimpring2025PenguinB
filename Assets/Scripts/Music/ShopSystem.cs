using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopCanvas; // Assign the Shop UI Canvas in the Inspector

    private void Start()
    {
        // Initially, ensure the shop canvas is disabled (not visible)
        shopCanvas.SetActive(false);
    }

    private void Update()
    {
        // Check for the 'E' key press to toggle the shop canvas visibility
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the shop canvas visibility
            bool isShopOpen = !shopCanvas.activeSelf;
            shopCanvas.SetActive(isShopOpen);

            // Switch music based on whether the shop is open or closed
            if (MusicManager.Instance != null)
            {
                if (isShopOpen)
                {
                    Debug.Log("Opening Shop - Switching to Shop Music");
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.shopMusic); // Play shop music
                }
                else
                {
                    Debug.Log("Closing Shop - Switching to Default Music");
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.defaultMusic); // Play default music
                }
            }
            else
            {
                Debug.LogError("MusicManager Instance is NULL! Check if it's in the scene.");
            }
        }
    }
}