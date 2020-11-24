using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Image healthbar;
    public GameObject lowHealth;
    public Image fadeImage;
    public TextMeshProUGUI text;
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
        text.text = current + "/" + max;
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
