using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{

    public Button startButton;
    private bool flicker = true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Flicker");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public IEnumerator Flicker()
    {
        while (flicker)
        {
            startButton.gameObject.SetActive(false); 
            yield return new WaitForSeconds(.5f);
            startButton.gameObject.SetActive(true); 
            yield return new WaitForSeconds(.5f);
        }
    }
}
