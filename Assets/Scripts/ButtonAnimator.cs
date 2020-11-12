using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{

    public Button button;
    public Image fadeImage;
    private bool flicker = true;
    
    void Start()
    {
        StartCoroutine("Flicker");
        button.onClick.AddListener(LoadNextScene);
    }

    IEnumerator Flicker()
    {
        while (flicker)
        {
            // Set transparent
            var color1 = button.gameObject.GetComponent<Image>().color;
            color1.a = 0;
            button.gameObject.GetComponent<Image>().color = color1;
            var color2 = button.gameObject.GetComponentInChildren<Text>().color;
            color2.a = 0;
            button.gameObject.GetComponentInChildren<Text>().color = color2;
            // Wait 
            yield return new WaitForSeconds(.5f);
            // Set opaque
            var color3 = button.gameObject.GetComponent<Image>().color;
            color3.a = 1;
            button.gameObject.GetComponent<Image>().color = color3;
            var color4 = button.gameObject.GetComponentInChildren<Text>().color;
            color4.a = 1;
            button.gameObject.GetComponentInChildren<Text>().color = color4;
            // Wait 
            yield return new WaitForSeconds(.5f);
        }
    }

    void LoadNextScene()
    {
        StartCoroutine("NextScene");
    }
    
    IEnumerator NextScene()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
