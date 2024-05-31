using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedLever : MonoBehaviour
{
    public Platform_one_movement platform;
    public ItemData key;
    public InventorySystem inventory;
    void Start() {
        platform.movementSpeed = 0;
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        bool result = inventory.UseItem(key);
        if (result)
        {
            platform.movementSpeed = 5;
            // play animation
        } else {
            // potentially add dialogue box, etc. here on screen
        }            
    }
}
