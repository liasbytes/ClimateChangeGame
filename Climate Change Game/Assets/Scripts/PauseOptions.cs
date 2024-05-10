using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseOptions : MonoBehaviour
{
    public FadeUI blackScreen;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    InputAction escape;

    void Start()
    {   
        blackScreen.gameObject.SetActive(true);
        blackScreen.DelayedFadeOut((float)0.2);
        escape = InputSystem.actions.FindAction("Escape");
    }

    // Update is called once per frame
    void Update()
    {
        if (escape.WasPressedThisFrame())
        {
            if (GameIsPaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void PauseGame() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitToHome()
    {
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu() {
        blackScreen.FadeIn();
        yield return new WaitForSecondsRealtime((float)0.4);
        SceneManager.LoadScene("StartMenu");
    }
}
