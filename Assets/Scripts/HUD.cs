using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthbar;
    public GameObject lowHealth;
    public Image fadeImage;
    float current;
    float max;

    void Start()
    {
        StartCoroutine("FadeIn");
    }
    
    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }
    
    public void UpdateHealth(float newhealth)
    {
        current = newhealth;
        float percentage = current / max;
        healthbar.fillAmount = percentage;
        if (percentage <= 0.25)
        {
            healthbar.color = Color.red;
        }
        else if (percentage <= 0.5)
        {
            healthbar.color = Color.yellow;
        }
        else
        {
            healthbar.color = Color.green;
        }
    }

    public void SetMax(float newmax)
    {
        max = newmax;
    }

    public void LowHealth()
    {
        lowHealth.gameObject.SetActive(true);
    }
}
