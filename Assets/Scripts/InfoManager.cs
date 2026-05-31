using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public int score { get; private set; }

    [Header("UI Reference")]
    public TMP_Text scoreText;
    public TMP_Text ballsText; 


    public void UpdateScoreUI(int newScore)
    {
        if(scoreText != null)
        {
            scoreText.text = "SCORE: " + newScore; 
        }
    }

    public void UpdateBallsUI(int BallsRemaining)
    {
        if(ballsText != null)
        {
            ballsText.text = "BALLS: " + BallsRemaining; 
        }
    }
}
