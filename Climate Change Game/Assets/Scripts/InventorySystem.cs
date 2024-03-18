using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<ItemData> inventory;

    // Update is called once per frame
    public void AddToInventory(ItemData data)
    {
        inventory.Add(data);
    }

    public bool UseItem(ItemData data) 
    {
        if (inventory.Contains(data))
        {
            inventory.Remove(data);
            return true;
        } else 
        {
            return false;
        }        
    }
}
