using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthbar;
    float current;
    float max;

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
}
