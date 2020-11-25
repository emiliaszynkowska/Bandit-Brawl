using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevel;
    public SoundManager sound;
    public Player player;
    public Character enemy;
    public GameObject fightText;
    public GameObject endScreen;
    public TextMeshProUGUI winText;
    public Image fadeImage;
    
    void Start()
    {
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        GameObject enemyObject = GameObject.Find("Enemy");
        enemy = enemyObject.GetComponent<Enemy>();
        if (enemy==null) enemy = enemyObject.GetComponent<Enemy2>();
        player.canMove = false; enemy.canMove = false;
        if (fadeImage != null) StartCoroutine(FadeIn());
        if (currentLevel != 1)
            StartGame();
    }

    public void StartGame()
    {
        StartCoroutine("StartGameCoroutine");
    }
    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        sound.PlayMusic();
        sound.PlayFight();
        StartCoroutine(DisplayFightText());
        yield return new WaitForSeconds(1f);
        player.canMove = true;
        enemy.canMove = true;
        //repeat until player dies
        while (player.health > 0 && enemy.health > 0)
        {
            yield return new WaitForSeconds(.1f);
        }
        //end of the game
        sound.StopMusic();
        player.canMove = false;
        enemy.canMove = false;
        if (player.health <= 0) //lose
        {
            sound.PlayDeath();
            yield return new WaitForSeconds(1f);
            endScreen.SetActive(true);
            winText.text = "You lose...";
        }
        else if (enemy.health <= 0) //win
        {
            sound.PlayWin(); 
            yield return new WaitForSeconds(1f);
            endScreen.SetActive(true);
            winText.text = "You win!";
            GameData.SaveLevel(currentLevel+1);
        }
        else //draw
        {
            sound.PlayDeath(); 
            yield return new WaitForSeconds(1f);
            endScreen.SetActive(true);
            winText.text = "It was a draw.";
        }

    }

    IEnumerator DisplayFightText()
    {
        fightText.SetActive(true);
        yield return new WaitForSeconds(1);
        fightText.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }

    public void LoadHome()
    {
        StartCoroutine(Load("Start"));
    }

    public void Replay()
    {
        StartCoroutine( Load(SceneManager.GetActiveScene().name));
    }

    public void Next()
    {
        StartCoroutine("LoadNext");
    }


    IEnumerator Load(string scene)
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
    }
    
    IEnumerator LoadNext()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(0.0f);
        fadeImage.CrossFadeAlpha(1.0f, 1, false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
