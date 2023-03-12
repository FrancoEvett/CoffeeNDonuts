using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// static class that keeps track of score and related fucntions, like saving the score on device
/// allows any oter class to add score points to the player and trigger a save when needed
/// </summary>


public static class Score
{
    private static int score;       //current game sessions score, will rest when a new session starts

    public static void AddScore(int value) { score += value; }
    public static int GetScore() { return score; }



    public static void SaveScore()
    {        
        if (PlayerPrefs.HasKey("HighScore"))
        {
            //call this method when the player dioes and has a final score
            if (score >= PlayerPrefs.GetInt("HighScore"))
            {
                //if this is a new high score then save the new score
                PlayerPrefs.SetInt("HighScore", score);
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }       
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public static void Reset()
    {
        score = 0;
    }
}
