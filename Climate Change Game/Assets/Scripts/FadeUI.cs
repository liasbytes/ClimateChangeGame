using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// with some help from https://youtu.be/tF9RMjF9wDc?si=W6Hpcx6ZKqzHWfVE and Unity documentation on WaitForSeconds
public class FadeUI : MonoBehaviour
{
    private CanvasGroup UIgroup;
    private int fadeInSpeed = 3;
    private int fadeOutSpeed = 5;
    private bool fadeIn = false;
    private bool fadeOut = false;

    void Start() {
        UIgroup = GetComponent<CanvasGroup>();
        if (UIgroup == null) {
            UIgroup = gameObject.AddComponent<CanvasGroup>();
        }        
    }

    void Update() {
        if (fadeIn) {
            if (UIgroup.alpha < 1) {
                UIgroup.alpha += fadeInSpeed*Time.unscaledDeltaTime;
                if (UIgroup.alpha >= 1) {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut) {
            if (UIgroup.alpha >= 0) {
                UIgroup.alpha -= fadeOutSpeed*Time.unscaledDeltaTime;
                if (UIgroup.alpha <= 0) {
                    fadeOut = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void FadeOut() {
        fadeOut = true;
    }

    public void FadeIn() {
        gameObject.SetActive(true);
        fadeIn = true;
    }

    public void DelayedFadeOut(float seconds) {
        StartCoroutine(waitOut(seconds));
    }

    public void DelayedFadeIn(float seconds) {
        gameObject.SetActive(true);
        StartCoroutine(waitIn(seconds));
    }

    IEnumerator waitOut(float seconds) {
        yield return new WaitForSeconds(seconds);
        fadeOut = true;
    }

    IEnumerator waitIn(float seconds) {
        yield return new WaitForSeconds(seconds);
        fadeIn = true;
    }
}


