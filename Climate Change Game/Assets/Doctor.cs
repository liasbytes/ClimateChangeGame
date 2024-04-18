using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Doctor : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] string[] dialogue;
    private int index;
    [SerializeField] float wordSpeed;
    public bool playerIsClose;

    [SerializeField] GameObject contButton;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text="";
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsClose) {
            if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeInHierarchy) {
                zeroText();
                foreach( var x in dialogue) Debug.Log( x.ToString());
            } else {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index]) {
            contButton.SetActive(true);
        }
    }

    public void zeroText() {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() {
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine() {
        contButton.SetActive(false);

        if (index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsClose = true;

        } else {
            zeroText();
        }
    }

    /** private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsClose = false;
            zeroText();
        }
    } */
}
