using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public InventorySystem inventory;
    public string title;
    public ItemData reference;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            inventory.AddToInventory(reference);
            Destroy(gameObject);
        }
    }
}
