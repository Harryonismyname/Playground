using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private UIHandler UIHandler;
    private ScoreHandler scoreHandler;
    private BallHandler ballHandler;
    [Header("Gamestate Settings")]
    public bool gameStart;
    public bool gameEnd;
    public bool gameRunning;
    public bool isPaused;
    [Header("Game Mode Settings")]
    public bool multiplayer;
    [Header("Difficulty Settings")]
    public int gameDifficulty = 0;

    private void Start()
    {
        UIHandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
        scoreHandler = GameObject.Find("ScoreHandler").GetComponent<ScoreHandler>();
        ballHandler = GameObject.Find("BallHandler").GetComponent<BallHandler>();
        isPaused = false;
        gameRunning = false;
        ballHandler.CreateBall();
    }
    private void Update()
    {
        if (gameRunning)
        {
            gameStart = false;
            if (scoreHandler.VictoryCheck())
            {
                EndGame();
            }
            if (ballHandler.BallCheck() == 1)
            {
                scoreHandler.Score(true);
                UIHandler.UpdateScoreDisplay(scoreHandler.playerScore, scoreHandler.opponentScore);
                ballHandler.ResetBall();
            }
            if (ballHandler.BallCheck() == 2)
            {
                scoreHandler.Score(false);
                UIHandler.UpdateScoreDisplay(scoreHandler.playerScore, scoreHandler.opponentScore);
                ballHandler.ResetBall();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        if (!gameRunning)
        {
            gameEnd = false;
            if (ballHandler.BallCheck() == 1 || ballHandler.BallCheck() == 2)
            {
                ballHandler.ResetBall();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIHandler.GoBack();
            }
        }
    }
    public void ExitToMain()
    {
        gameEnd = true;
        gameRunning = false;
        TogglePause();
        UIHandler.Main();
        ballHandler.ResetBall();
    }
    /// <summary>Begins core gameplay loop at the passed difficulty value</summary>
    public void StartGame(int difficulty = 0)
    {
        ballHandler.DestroyBall();
        gameStart = true;
        if (difficulty > 0)
        {
            gameDifficulty = difficulty;
        }
        if (difficulty == 0)
        {
            multiplayer = true;
        }
        scoreHandler.ResetScore();
        UIHandler.UpdateScoreDisplay(scoreHandler.playerScore, scoreHandler.opponentScore);
        UIHandler.Play();
        gameRunning = true;
        ballHandler.CreateBall();
    }
    /// <summary>Ends all gameplay based processes</summary>
    private void EndGame()
    {
        gameEnd = true;
        gameRunning = false;
        UIHandler.Victory();
        UIHandler.DeclareWinner(scoreHandler.DetermineVictor());
        ballHandler.ResetBall();
    }
    /// <summary>Toggles the game between paused and un-paused</summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        if (!isPaused)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
        UIHandler.TogglePauseMenu();
    }
    public void ExitUnity()
    {
        Application.Quit();
    }
}
