using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedArea : MonoBehaviour
{
    public ItemData key;
    public InventorySystem inventory;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
        bool result = inventory.UseItem(key);
        if (result)
        {
            Destroy(gameObject);
        } else {
            Debug.Log("Not too fast! You need '" + key.name + "' to pass.");
            // potentially add dialogue box, etc. here on screen
        }            
    }
}
