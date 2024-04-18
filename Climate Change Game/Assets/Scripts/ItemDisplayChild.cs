using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayChild : MonoBehaviour
{
    public ItemData item;
    // Start is called before the first frame update
    void Start()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = item.sprite;
    }

    // Update is called once per frame
    public bool isFound()
    {
        return item.found;
    }

    public string getName()
    {
        return item.name;
    }

    public string getDesc()
    {
        return item.description;
    }

    public string getLoc()
    {
        return item.levelLocation;
    }
}
