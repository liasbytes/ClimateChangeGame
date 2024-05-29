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
    public DataManager dataManager;

    void Start()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.DelayedFadeOut((float)0.1);
        Time.timeScale = 1f;
    }
    public void StartLevel()
    {   
        Time.timeScale = 1f;
        dataManager.loading = false;
        StartCoroutine(fadeAndLoad());
    }

    public void LoadLevel()
    {
        dataManager.loading = true;
        Time.timeScale = 1f;
        StartCoroutine(fadeAndLoad());
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
