using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelButton;
    public Transform lockImg;

    void Start()
    {
        lockImg = transform.Find("Lock");
        Button button = GetComponent<Button>();
        int current = GameData.LoadLevel();
        Debug.Log(current + "level is unlocked");
        if (current>=levelButton)
        {
            button.interactable = true;
            if (lockImg != null) lockImg.gameObject.SetActive(false);
        }
        else
        {
            button.interactable = false;
            if (lockImg != null) lockImg.gameObject.SetActive(true);
        }
    }
}
