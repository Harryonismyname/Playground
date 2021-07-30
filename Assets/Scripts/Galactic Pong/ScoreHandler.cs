using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Score Settings")]
    public int playerScore = 0;
    public int opponentScore = 0;
    private readonly int victoryCondition = 10;
    
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
    public string DetermineVictor()
    {
        if(playerScore >= victoryCondition)
        {
            return "Player 1";
        }
        else
        {
            return "Player 2";
        }
    }

    public void ResetScore()
    {
        playerScore = 0;
        opponentScore = 0;
    }

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
