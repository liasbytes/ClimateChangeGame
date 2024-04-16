using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string name;
    public string description;
    public string levelLocation;
    public Sprite sprite;
    public bool found = false;

    public void setFound() {
        found = true;
    }
}
