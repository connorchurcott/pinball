using System.Threading;
using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public int score { get; private set; }

    [Header("UI Reference")]
    public TMP_Text timerText; 
    public TMP_Text scoreText;
    public TMP_Text ballsText; 
    public TMP_Text targetScoreText; 
    

    public void UpdateTimerUI(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f); 
        int seconds = Mathf.FloorToInt(timeRemaining % 60f); 
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds); 
    }

    public void UpdateScoreUI(int newScore)
    {
        if(scoreText != null)
        {
            scoreText.text = "SCORE: " + newScore; 
        }
    }

    public void UpdateTargetScoreUI(int newTargetScore)
    {
        targetScoreText.text = "TARGET: " + newTargetScore; 
    }

    public void UpdateBallsUI(int BallsRemaining)
    {
        if(ballsText != null)
        {
            ballsText.text = "BALLS: " + BallsRemaining; 
        }
    }
}
