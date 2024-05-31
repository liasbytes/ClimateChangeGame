using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text infoTitle;
    public TMP_Text infoDescription;
    public ItemDisplayChild item;
    public ItemDisplayChild unknown;
    private ItemDisplayChild shownItem;
    // Start is called before the first frame update
    void Start()
    {
        if (item.isFound()) {
            item.gameObject.SetActive(true);
            shownItem = item;
        }
        else {
            unknown.gameObject.SetActive(true);
            shownItem = unknown;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = shownItem.getName();
        string description = shownItem.getDesc();
        string location = shownItem.getLoc();
        // change text in the info box about this item
        infoTitle.text = name;
        infoDescription.text = description + "\nFound in: " + location;
        infoTitle.gameObject.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoTitle.gameObject.SetActive(false);
    }
}