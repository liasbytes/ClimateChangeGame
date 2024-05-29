using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// File writing code adapted from https://github.com/UnityTechnologies/UniteNow20-Persistent-Data
public class DataManager : MonoBehaviour
{
    public BasicPlayerController playerData;
    public InventorySystem inventoryData;

    public bool loading;
    public bool permanent;

    public void OnEnable()
    {
        if (!permanent)
        {
            SceneManager.sceneLoaded += onSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
        loading = false;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CityLevel")
        {
            if (loading == true)
            {
                Debug.Log(GameObject.FindGameObjectsWithTag("Player"));
                playerData = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<BasicPlayerController>();
                inventoryData = GameObject.FindGameObjectsWithTag("InventoryManager")[0].GetComponent<InventorySystem>();
                readSaveData();
                //Debug.Log("Loaded City Level");
            }
            SceneManager.sceneLoaded -= onSceneLoaded;
            Destroy(this.gameObject);
        }
    }

    public void writeSaveData()
    {
        SaveData data = new SaveData();
        data.health = playerData.health;
        data.cpNum = playerData.cpNum;
        data.checkpointLocation = playerData.respawnLocation.position;
        data.inventory = inventoryData.inventory;
        string JSONData = JsonUtility.ToJson(data);
        var fullPath = Path.Combine(Application.persistentDataPath, "ClimateChangeSaveData.json");
        try
        {
            File.WriteAllText(fullPath, JSONData);
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
        SaveData data = JsonUtility.FromJson<SaveData>(JSONData);
        playerData.health = data.health;
        playerData.cpNum = data.cpNum;
        playerData.respawnLocation.position = data.checkpointLocation;
        playerData.transform.position = data.checkpointLocation;
        inventoryData.inventory = data.inventory;
    }

}
