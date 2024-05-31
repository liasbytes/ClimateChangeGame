using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    public void QuitToHome()
    {
        StartCoroutine(LoadMenu());
    }

    public void FullQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime((float)0.4);
        SceneManager.LoadScene("StartMenu");
    }
}
