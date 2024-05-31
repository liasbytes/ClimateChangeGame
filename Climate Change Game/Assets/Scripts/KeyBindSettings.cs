using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// from tutorial: https://youtu.be/MEHe0Uf3m4I?si=MicKS0-i-20-7Wf4

public class KeyBindSettings : MonoBehaviour
{
    private Dictionary<string, KeyCode> keyDict = new Dictionary<string, KeyCode>();
    public TMP_Text left, right, jump, attack;
    private GameObject currentKey = null;

    private Color32 normal = new Color32(94,94,94,255);
    private Color32 selected = new Color32(239,116,40,255);

    public InputActionAsset actions;

    // Start is called before the first frame update
    void Start()
    {
        // typecasting after loading keybinds from playerprefs
        keyDict.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Left","A")));  
        keyDict.Add("Right",(KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Right","D")));
        keyDict.Add("Jump", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Jump","Space")));
        keyDict.Add("Attack", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Attack","Q")));

        left.text = keyDict["Left"].ToString();
        right.text = keyDict["Right"].ToString();
        jump.text = keyDict["Jump"].ToString();
        attack.text = keyDict["Attack"].ToString();
    }


    void OnGUI()
    {
        if (currentKey != null) 
        {
            Event e = Event.current;
            if (e.isKey) {
                keyDict[currentKey.name] = e.keyCode; // bind selected key with new key input
                currentKey.transform.GetChild(0).GetComponent<TMP_Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked) 
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal; // return previous selected to normal
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
