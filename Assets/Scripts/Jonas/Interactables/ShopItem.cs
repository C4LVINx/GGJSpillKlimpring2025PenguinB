using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName; // Name of the item
    public int cost; // Cost of the item in terms of stored objects or coins
    public Button purchaseButton; // Button to buy the item
}