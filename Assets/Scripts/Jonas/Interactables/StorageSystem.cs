using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    [Header("Storage Settings")]
    public List<GameObject> storedObjects = new List<GameObject>();  // List to store objects
    public int yuzuCoins = 0; // The YuzuCoin currency

    // Method to store a caught object
    public void StoreCaughtObject(GameObject caughtObject)
    {
        storedObjects.Add(caughtObject);
        Debug.Log($"{caughtObject.name} has been stored in the storage system.");
    }

    // Method to add YuzuCoins
    public void AddYuzuCoins(int amount)
    {
        yuzuCoins += amount;
        Debug.Log($"Added {amount} YuzuCoins. Current balance: {yuzuCoins}.");
    }

    // Method to spend YuzuCoins
    public bool SpendYuzuCoins(int amount)
    {
        if (yuzuCoins >= amount)
        {
            yuzuCoins -= amount;
            Debug.Log($"Spent {amount} YuzuCoins. Remaining balance: {yuzuCoins}.");
            return true;
        }
        else
        {
            Debug.Log("Not enough YuzuCoins.");
            return false;
        }
    }

    // Method to remove a specific object from storage
    public void RemoveObject(GameObject objectToRemove)
    {
        if (storedObjects.Contains(objectToRemove))
        {
            storedObjects.Remove(objectToRemove);
            Debug.Log($"{objectToRemove.name} has been removed from storage.");
        }
    }

    // Method to get the count of stored objects
    public int GetStoredObjectCount()
    {
        return storedObjects.Count;
    }
}