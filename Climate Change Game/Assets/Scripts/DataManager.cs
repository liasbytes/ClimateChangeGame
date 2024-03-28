using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Some file writing code adapted from https://github.com/UnityTechnologies/UniteNow20-Persistent-Data
public class DataManager : MonoBehaviour
{
    public PlayerController playerData;
    public InventorySystem inventoryData;

    public void writeSaveData()
    {
        Debug.Log("Test");
        SaveData data = new SaveData();
        data.health = playerData.health;
        data.cpNum = playerData.cpNum;
        data.checkpointLocation = playerData.respawnLocation;
        data.inventory = inventoryData.inventory;
        string JSONData = JsonUtility.ToJson(data);
        Debug.Log(JSONData);
        var fullPath = Path.Combine(Application.persistentDataPath, "ClimateChangeSaveData.json");
        try
        {
            File.WriteAllText(JSONData, fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
        }
        
    }

    public void readSaveData()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, "ClimateChangeSaveData.json");
        string JSONData = "";
        try
        {
            JSONData = File.ReadAllText(fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            return;
        }
        Debug.Log(JSONData);
        SaveData data = JsonUtility.FromJson<SaveData>(JSONData);
        playerData.health = data.health;
        playerData.cpNum = data.cpNum;
        playerData.respawnLocation = data.checkpointLocation;
        playerData.transform.position = data.checkpointLocation.position;
        inventoryData.inventory = data.inventory;

    }

}
