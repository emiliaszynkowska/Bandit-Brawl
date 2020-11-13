using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject menu;
    public Button startButton;
    public Button homeButton;
    public GameObject wasd;
    public GameObject mouse;
    private Animator wasdAnimator;
    private Animator mouseAnimator;
    public Image fadeImage;
    
    void Start()
    {
        StartCoroutine("FadeIn");
        startButton.onClick.AddListener(Tutorial);
        homeButton.onClick.AddListener(LoadTitle);
        wasdAnimator = wasd.GetComponent<Animator>();
        mouseAnimator = mouse.GetComponent<Animator>();
    }

    void Tutorial()
    {
        menu.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        LearnMovement();
        LearnAttack();
        LearnBlock();
    }

    void LearnMovement()
    {
        wasd.SetActive(true);
        wasd.SetActive(false);
    }

    void LearnAttack()
    {
        mouse.SetActive(true);
        mouseAnimator.SetTrigger("LeftClick");
        mouse.SetActive(false);
    }

    void LearnBlock()
    {
        mouse.SetActive(true);
        mouseAnimator.SetTrigger("RightClick");
        mouse.SetActive(false);
    }

    void LoadTitle()
    {
        StartCoroutine("Title");
    }
    
    IEnumerator Title()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Start");
    }
    
    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }
    
}
