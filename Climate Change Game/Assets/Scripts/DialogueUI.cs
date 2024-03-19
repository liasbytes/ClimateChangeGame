using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour {
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject player;
    private void Start() {
        textLabel.text = "default text.";
    }
    void Update() {
        if (player.GetComponent<PlayerController>().GetCollisions()) {
        //SetColor("_Color",Color.red);
        textLabel.text = "collision detected.";
        }
    }
}