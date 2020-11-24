using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHUD : MonoBehaviour
{
    public GameManager game;
    public Enemy enemy;
    public GameObject dark;
    public GameObject playerGlow;
    public GameObject enemyGlow;
    public GameObject playerTutorial;
    public GameObject enemyTutorial;
    
    void Start()
    {
        PlayerTutorial();
    }

    public void PlayerTutorial()
    {
        // Show components
        dark.SetActive(true);
        playerGlow.SetActive(true);
        playerTutorial.SetActive(true);
    }

    public void EnemyTutorial()
    {
        // Hide components
        playerGlow.SetActive(false);
        playerTutorial.SetActive(false);
        // Show components
        enemyGlow.SetActive(true);
        enemyTutorial.SetActive(true);
    }

    public void End()
    {
        // Hide Components
        enemyGlow.SetActive(false);
        enemyTutorial.SetActive(false);
        dark.SetActive(false);
        game.StartGame();
    }
    
}
