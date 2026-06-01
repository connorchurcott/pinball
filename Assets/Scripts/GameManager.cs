using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int startingBalls = 2; 
    public int targetScore = 10000; 
    public float startingTime = 240f; 

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
    public int CurrentScreen { get; private set; }

    float timeRemaining; 
    bool timerRunning; 
    GameObject currentBall; 

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
        // set not game over
        IsGameOver = false; 
        gameOverScreen.SetActive(false); 

        // set starting balls
        BallsRemaining = startingBalls; 
        infoManager.UpdateBallsUI(BallsRemaining);

        // set score, target score 
        Score = 0; 
        infoManager.UpdateScoreUI(Score); 
        infoManager.UpdateTargetScoreUI(targetScore); 

        // set time
        timerRunning = true; 
        timeRemaining = startingTime; 
        infoManager.UpdateTimerUI(timeRemaining);

        // set current screen and spawn first ball
        CurrentScreen = 1; 
        SpawnBallInLauncher(); 
    }

    void Update()
    {
        // don't run the timer if its not supposed to be running, like if the games paused
        if (!timerRunning)
        {
            return; 
        }

        // Constantly update time, and check if its less than 0, if so then end the game 
        timeRemaining -= Time.deltaTime; 
        infoManager.UpdateTimerUI(timeRemaining);

        if(timeRemaining <= 0)
        {
            timeRemaining = 0; 
            infoManager.UpdateTimerUI(timeRemaining); 
            TriggerGameOver(); 
        }
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
        CheckWinCondition(); 
    }

    void CheckWinCondition()
    {
        if(Score >= targetScore)
        {
            LoadNextScreen(); 
        }
    }

    void LoadNextScreen()
    {
        timerRunning = false; 
        CurrentScreen++; 
        Debug.Log("Screen Cleared, load next screen"); 

        // add screen transition + resetting for next screen (hopefully soon :p)
        // for now just increase target and keep playing 
        targetScore += 5000 * CurrentScreen; 
        infoManager.UpdateTargetScoreUI(targetScore);
        timeRemaining = startingTime - 10 * CurrentScreen;  
        infoManager.UpdateTimerUI(timeRemaining); 

        if (currentBall)
        {
            Destroy(currentBall); 
        }
        SpawnBallInLauncher(); 
        timerRunning = true; 
    }

    // creates new ball, and tells launcher that there is a new ball so it can updates it's ballRB and bar settings. 
    public void SpawnBallInLauncher()
    {
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
        currentBall = newBall; 
        Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>(); 
        ballLauncher.SetBall(newBallRB); 
    }

    public void TriggerGameOver()
    {
        IsGameOver = true; 
        timerRunning = false; 
        gameOverScreen.SetActive(true); 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }



}
