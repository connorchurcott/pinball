using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int startingBalls = 2; 

    [Header("References (UI)")]
    public GameObject gameOverScreen; 
    public InfoManager infoManager; 

    [Header("References (Ball Spawning)")]
    public GameObject ballPrefab; 
    public BallLauncher ballLauncher; 
    public Transform ballSpawnPosition; 

    
    public int Score { get; private set; }
    public int BallsRemaining { get; private set; }
    public bool IsGameOver { get; private set; }

    void Awake()
    {
         // singleton allowing anything to access GameManager.Instance
         if(Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return; 
        }
        Instance = this; 
    }

    void Start()
    {
        IsGameOver = false; 
        gameOverScreen.SetActive(false); 

        BallsRemaining = startingBalls; 
        infoManager.UpdateBallsUI(BallsRemaining);
        SpawnBallInLauncher(); 

    }

    public void OnBallDrained(GameObject ballDrained)
    {
        BallsRemaining--;
        infoManager.UpdateBallsUI(BallsRemaining); 

        if(BallsRemaining <= 0)
        {
            TriggerGameOver(); 
        }
        else
        {
            float timeToDrestoyBall = 2.0f; 
            Destroy(ballDrained, timeToDrestoyBall); 
            SpawnBallInLauncher(); 
        }
    }

    public void IncrementScore(float points)
    {
        Score += Mathf.RoundToInt(points); 
        infoManager.UpdateScoreUI(Score); 
    }

    // creates new ball, and tells launcher that there is a new ball so it can updates it's ballRB and bar settings. 
    public void SpawnBallInLauncher()
    {
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
        Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>(); 
        ballLauncher.SetBall(newBallRB); 
    }

    public void TriggerGameOver()
    {
        IsGameOver = true; 
        gameOverScreen.SetActive(true); 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }



}
