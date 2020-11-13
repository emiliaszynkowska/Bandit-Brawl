using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public GameObject title;
    public Button startButton;
    public GameObject menu;
    public Button homeButton;
    public Button tutorialButton;
    public Button levelButton;
    public Image fadeImage;
    private bool flicker = true;
    
    void Start()
    {
        StartCoroutine("FadeIn");
        StartCoroutine("Flicker");
        startButton.onClick.AddListener(ShowMenu);
        homeButton.onClick.AddListener(HideMenu);
        tutorialButton.onClick.AddListener(LoadTutorial);
        levelButton.onClick.AddListener(LoadLevel);
    }

    IEnumerator Flicker()
    {
        while (flicker)
        {
            // Set transparent
            var color1 = startButton.gameObject.GetComponent<Image>().color;
            color1.a = 0;
            startButton.gameObject.GetComponent<Image>().color = color1;
            var color2 = startButton.gameObject.GetComponentInChildren<Text>().color;
            color2.a = 0;
            startButton.gameObject.GetComponentInChildren<Text>().color = color2;
            // Wait 
            yield return new WaitForSeconds(.5f);
            // Set opaque
            var color3 = startButton.gameObject.GetComponent<Image>().color;
            color3.a = 1;
            startButton.gameObject.GetComponent<Image>().color = color3;
            var color4 = startButton.gameObject.GetComponentInChildren<Text>().color;
            color4.a = 1;
            startButton.gameObject.GetComponentInChildren<Text>().color = color4;
            // Wait 
            yield return new WaitForSeconds(.5f);
        }
    }

    void ShowMenu()
    {
        title.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    void HideMenu()
    {
        title.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }

    void LoadTutorial()
    {
        StartCoroutine("Tutorial");
    }

    void LoadLevel()
    {
        StartCoroutine("Level");
    }
    
    IEnumerator Tutorial()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Tutorial");
    }
    
    IEnumerator Level()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level");
    }
    
    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }
    
}
