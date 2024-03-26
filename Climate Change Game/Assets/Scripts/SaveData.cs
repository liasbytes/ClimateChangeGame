using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float health;
    public int cpNum;
    public Transform checkpointLocation;
    public List<ItemData> inventory;

}
