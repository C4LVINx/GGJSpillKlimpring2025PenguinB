using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    [Header("Storage Settings")]
    public List<GameObject> storedObjects = new List<GameObject>(); // List to store the caught objects

    public void StoreCaughtObject(GameObject caughtObject)
    {
        // Store the object in the inventory
        storedObjects.Add(caughtObject);
        Debug.Log($"Stored {caughtObject.name} in inventory.");
    }

    public void ShowStoredObjects()
    {
        // Display all stored objects for debugging
        Debug.Log("Stored Objects: ");
        foreach (var obj in storedObjects)
        {
            Debug.Log(obj.name);
        }
    }
}