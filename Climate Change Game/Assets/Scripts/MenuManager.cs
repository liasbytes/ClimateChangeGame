using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public FadeUI blackScreen;
    void Start()
    {
        Time.timeScale = 1f;
    }
    public void StartLevel()
    {
        StartCoroutine(fadeAndLoad());
        Time.timeScale = 1f;
    }

    public void LoadLevel()
    {
        // add code?
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    IEnumerator fadeAndLoad() {
        blackScreen.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("CityLevel"); // from load game page: should load to specified checkpoint
    }

}
