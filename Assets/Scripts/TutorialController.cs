using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    // Main menu 
    public GameObject menu;
    public Button startButton;
    public Button homeButton;
    // Other menus
    public Button returnButton;
    public GameObject complete;
    public GameObject end;
    public Button endButton;
    // Learning
    public GameObject movement;
    public GameObject jump;
    public GameObject attack;
    public GameObject slam;
    public GameObject block;
    // Assets
    public Sprite check;
    public Sprite uncheck;
    public GameObject wasd;
    public GameObject space;
    public GameObject mouse;
    public GameObject leftSpace;
    public GameObject rightMouse;
    private Animator mouseAnimator;
    public Image fadeImage;
    [SerializeField] protected SoundManager sound;

    void Start()
    {
        StartCoroutine("FadeIn");
        startButton.onClick.AddListener(LearnMovement);
        homeButton.onClick.AddListener(LoadTitle);
        returnButton.onClick.AddListener(LoadTitle);
        endButton.onClick.AddListener(LoadTitle);
        mouseAnimator = mouse.GetComponent<Animator>();
    }

    public void LearnMovement()
    {
        // Hide components
        menu.gameObject.SetActive(false);
        // Show components
        wasd.SetActive(true);
        movement.SetActive(true);
        complete.SetActive(true);
        returnButton.gameObject.SetActive(true);
    }

    public void LearnJump()
    {
        // Hide components
        wasd.SetActive(false);
        movement.SetActive(false);
        // Show components
        jump.SetActive(true);
        space.SetActive(true);
    }

    public void LearnAttack()
    {
        // Hide components
        jump.SetActive(false);
        space.SetActive(false);
        // Show components
        attack.SetActive(true);
        mouse.SetActive(true);
        mouseAnimator.SetTrigger("LeftClick");
        ResetComplete();
    }
    
    public void LearnSlam()
    {
        // Hide components
        attack.SetActive(false);
        mouse.SetActive(false);
        GameObject.Find("Platform 1").SetActive(false);
        GameObject.Find("Platform 2").SetActive(false);
        // Show components
        slam.SetActive(true);
        leftSpace.SetActive(true);
        rightMouse.SetActive(true);
        ResetComplete();
    }

    public void LearnBlock()
    {
        // Hide components
        slam.SetActive(false);
        leftSpace.SetActive(false);
        rightMouse.SetActive(false);
        // Show components
        mouse.SetActive(true);
        mouseAnimator.SetTrigger("RightClick");
        block.SetActive(true);
        ResetComplete();
    }

    public void EndTutorial()
    {
        // Hide components
        block.SetActive(false);
        mouse.SetActive(false);
        complete.SetActive(false);
        end.SetActive(true);
    }

    public void SetComplete(int completed)
    {
        sound.PlayRing();
        switch (completed)
        { 
            case (1):
                complete.GetComponentsInChildren<Image>()[0].sprite = check;
                break;
            case (2):
                complete.GetComponentsInChildren<Image>()[1].sprite = check;
                break;
            case (3):
                complete.GetComponentsInChildren<Image>()[2].sprite = check;
                break;
        }
    }

    void ResetComplete()
    {
        GameObject.Find("Complete 1").GetComponent<Image>().sprite = uncheck;
        GameObject.Find("Complete 2").GetComponent<Image>().sprite = uncheck;
        GameObject.Find("Complete 3").GetComponent<Image>().sprite = uncheck;
    }

    void LoadTitle()
    {
        StartCoroutine("Title");
    }

    public void DummyDamage(string dummy)
    {
        sound.PlayDamage();
        if (dummy.Equals("A"))
            GameObject.Find("Dummy A").GetComponent<Animator>().SetTrigger("Damage");
        else if (dummy.Equals("B"))
            GameObject.Find("Dummy B").GetComponent<Animator>().SetTrigger("Damage");
    }

    IEnumerator Title()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
    
    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }
    
}
