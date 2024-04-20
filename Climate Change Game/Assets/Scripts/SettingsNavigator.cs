using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsNavigator : MonoBehaviour
{
    public Button volumeButton, controlsButton, graphicsButton, advancedButton;
    public GameObject volume, controls, graphics, advanced;
    private List<Button> btnList;
    private List<GameObject> menuList;
    private Color selectedColor;
    private Color deselectColor;

    // Start is called before the first frame update
    void Start()
    {
        btnList = new List<Button>() {volumeButton, controlsButton, graphicsButton, advancedButton};
        menuList = new List<GameObject>() {volume, controls, graphics, advanced};
        selectedColor = new Color(180/255f,180/255f,180/255f, 100/255f);
        deselectColor = new Color(100/255f,100/255f,100/255f, 100/255f);

        SelectPage(volumeButton);
    }

    public void SelectPage(Button btn) {
        ClearSelected();
        menuList[btnList.IndexOf(btn)].SetActive(true);
        btn.GetComponent<Image>().color = selectedColor;
    }

    void ClearSelected() {
        foreach (Button btn in btnList) {
            btn.GetComponent<Image>().color = deselectColor;
        }
        foreach (GameObject g in menuList) {
            g.SetActive(false);
        }
    }
}
