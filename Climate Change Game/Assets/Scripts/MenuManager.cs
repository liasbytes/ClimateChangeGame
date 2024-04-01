using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CityLevel"); // eventually should load to specified checkpoint
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

}
