using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int startingBalls = 2; 
    public int targetScore = 10000; 
    public float startingTime = 240f; 
    public List<BallData> startingBallData;  

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
    List<GameObject> allBallsInPlay = new List<GameObject>(); 
    Queue<BallData> mainBallDataQueue = new Queue<BallData>(); 

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

        foreach(BallData bd in startingBallData)
        {
            mainBallDataQueue.Enqueue(bd); 
        }

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

    // Remove drained ball first. then if there are no balls in the array, it will cost a life. then
    // if that cost of life brings you down to 0 it ends the game 
    public void OnBallDrained(GameObject ballDrained)
    {
        // FOR NOW: when your ability ball gets drained, just enqueue that ability back at the end. 
        // later will have to figure out a system to permantly lose you your ability ball, maybe if you drain it enough times? 
        BallInstance instance = ballDrained.GetComponent<BallInstance>(); 
        if(instance != null && !instance.IsNormalBall)
        {
            mainBallDataQueue.Enqueue(instance.data); 
        }


        allBallsInPlay.Remove(ballDrained); 
        Destroy(ballDrained); 

        if(allBallsInPlay.Count == 0)
        {
            BallsRemaining--; 
            infoManager.UpdateBallsUI(BallsRemaining); 
            
            if(BallsRemaining <= 0)
            {
                TriggerGameOver(); 
            }
            else
            {
                SpawnBallInLauncher(); 
            }
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
        // for now just increase target, decrease time, reset balls, and keep playing 
        targetScore += 5000 * CurrentScreen; 
        infoManager.UpdateTargetScoreUI(targetScore);
        timeRemaining = startingTime - 10 * CurrentScreen;  
        infoManager.UpdateTimerUI(timeRemaining); 

        // clear all balls in play, first enque all main balls back
        foreach(GameObject ball in allBallsInPlay)
        {
            if(ball != null)
            {
                BallInstance instance = ball.GetComponent<BallInstance>(); 
                if(instance != null && !instance.IsNormalBall)
                {
                    mainBallDataQueue.Enqueue(instance.data);
                }

                Destroy(ball); 
            }
        }
        allBallsInPlay.Clear(); 

        infoManager.UpdateBallsUI(BallsRemaining); 
        SpawnBallInLauncher(); 
        timerRunning = true; 
    }

    // creates new ball, assigns it the next ballData in the queue. Used for "Main" balls that have abilities 
    public void SpawnBallInLauncher()
    {
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
        allBallsInPlay.Add(newBall); 

        // dequeues the next balls data, and assigns it to the new ball being created
        if(mainBallDataQueue.Count > 0)
        {
            BallInstance newBallInstance = newBall.GetComponent<BallInstance>(); 
            if(newBallInstance != null)
            {
                newBallInstance.data = mainBallDataQueue.Dequeue(); 
            }
        }

        Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>(); 
        ballLauncher.SetBall(newBallRB); 
    }

    // specifically used to spawn a normal ball (ususally for multiballs), spawn at the position of the current main ball usually, but
    // up to the caller? change later back to launcher if it feels bad
    public void SpawnNormalBall(Vector2 position)
    {
        GameObject newBall = Instantiate(ballPrefab, position, Quaternion.identity); 
        allBallsInPlay.Add(newBall); 
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
