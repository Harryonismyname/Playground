using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject main;
    [SerializeField] GameObject gameMode;
    [SerializeField] GameObject difficulty;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject playerUI;
    [Header("Score Settings")]
    public Text playerScoreText;
    public Text opponentScoreText;
    public Text winnerText;
    // Internal Values
    [SerializeField] private GameObject currentScreen;
    private GameObject[] screenTier;
    private int location;
    void Start()
    {
        currentScreen = main;
        screenTier = new GameObject[] { main, gameMode, difficulty, victory, playerUI };
        main.SetActive(true);
        location = 0;
        playerUI.SetActive(false);
        pauseMenu.SetActive(false);
        titleScreen.SetActive(true);
        victory.SetActive(false);
    }
    public void DeclareWinner(string name)
    {
        winnerText.text = name+" Wins!";
    }

    public void UpdateScoreDisplay(int playerScore, int opponentScore)
    {
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }
    private void UpdateLocation(GameObject screen, int loc)
    {
        currentScreen.SetActive(false);
        location = loc;
        screen.SetActive(true);
        currentScreen = screen;
    }
    public void GoBack()
    {
        location--;
        if (location < 0)
        {
            location = 0;
        }
        UpdateLocation(screenTier[location], location);
    }
    public void Main()
    {
        UpdateLocation(main, 0);
    }

    public void GameMode()
    {
        UpdateLocation(gameMode, 1);
    }

    public void Difficulty()
    {
        UpdateLocation(difficulty, 2);
    }

    public void Victory()
    {
        UpdateLocation(victory, 3);
    }

    public void Play()
    {
        UpdateLocation(playerUI, 4);
    }
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
