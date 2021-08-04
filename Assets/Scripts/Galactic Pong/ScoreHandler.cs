using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Score Settings")]
    public int playerScore = 0;
    public int opponentScore = 0;
    private readonly int victoryCondition = 10;
    /// <summary>Compares player and opponent scores and returns true if score exceeds victory conditions</summary>
    public bool VictoryCheck()
    {
        if (playerScore >= victoryCondition || opponentScore >= victoryCondition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>Compares player and opponent scores and returns name of player whose score exceeds victory conditions</summary>
    public string DetermineVictor()
    {
        if (playerScore >= victoryCondition)
        {
            return "Player 1";
        }
        else
        {
            return "Player 2";
        }
    }
    /// <summary>Sets scores to zero</summary>
    public void ResetScore()
    {
        playerScore = 0;
        opponentScore = 0;
    }
    /// <summary>Incriments score based on whose side of the court the ball had passed</summary>
    public void Score(bool playerPoint)
    {
        if (playerPoint)
        {
            playerScore++;
        }
        else
        {
            opponentScore++;
        }
    }
}
