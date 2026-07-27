using System.Collections;
using Unity.ProjectAuditor.Editor.Core;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BallInstance : MonoBehaviour
{
    [Header("Ball Settings")]
    public float maxVelocity = 10f; 


    public BallData data; 

    float cooldownRemaining = 0f; 
    public bool IsNormalBall { get; private set; }  

    PinballControls controls;

    private Rigidbody2D curBallRB;  
    private bool velocityLocked = false; 
    private float velocityLockedSpeed = 0f; 

    void Awake()
    {
        controls = new PinballControls(); 
    }

    void OnEnable()
    {
        controls?.Enable(); 
    }

    void OnDisable()
    {
        controls?.Disable();       
    }


    // assigns the ball the color that is chosen in data, checks if ability is none, if so then this is a normal ball
    void Start()
    {
        curBallRB = GetComponent<Rigidbody2D>(); 

        if(data == null || data.ability == BallAbility.None )
        {
            IsNormalBall = true; 
        }

        // for now, if there is a sprite render it otherwise just add the color 
        if(data != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>(); 
            if(sr != null)
            {
                if(data.ballSprite != null)
                {
                    sr.sprite = data.ballSprite; 
                }
                else
                {
                    sr.color = data.ballColor; 
                }
            }
        }       
    }

    void Update()
    {
        // SECTION FOR ALL BALLS



        // SECTION FOR ABILITY BALLS

        // if this is a standard ball or passive ball just return 
        if(IsNormalBall || data.activation != AbilityActivation.Active)
        {
            return; 
        }

        // reduce ability cooldown 
        if(cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime; 
        }

        // check if user inputed an ability 
        if(controls.Game.BallAbility.WasPressedThisFrame() && cooldownRemaining <= 0)
        {
            ActivateAbility(); 
        }
    }

    void FixedUpdate()
    {
        // for bullet time, currently used by SlowTimeBall
        if (velocityLocked)
        {
            curBallRB.linearVelocity = curBallRB.linearVelocity.normalized * velocityLockedSpeed; 
            return; 
        }

        if(curBallRB.linearVelocity.magnitude > maxVelocity)
        {
            curBallRB.linearVelocity = curBallRB.linearVelocity.normalized * maxVelocity; 
        }

    }

    void ActivateAbility()
    {

        // hard coded for SpawnOneMultiball, in future i gotta add functions for each ability and a enum or smthn 
        if(data.ability == BallAbility.SpawnOneMultiball)
        {
            GameManager.Instance.SpawnNormalBall(transform.position); 
            cooldownRemaining = data.cooldown; 
            return; 
        }

        if(data.ability == BallAbility.SlowDownSpeedOneSecond)
        {
            StartCoroutine(SlowDownSpeedOneSecondRoutine()); 
            cooldownRemaining = data.cooldown; 
            return; 
        }
    }


    // COROUTINES FOR DIFFERENT BALL ABILITIES
    IEnumerator SlowDownSpeedOneSecondRoutine()
    {
        const float SPEED_MULTIPLIER = 0.25f; 
        const float DAMPEN_TIME = 1.5f; 

        float originalSpeed = curBallRB.linearVelocity.magnitude; 
        float originalGravity = curBallRB.gravityScale; 


        curBallRB.gravityScale = 0f; 
        velocityLockedSpeed = originalSpeed * SPEED_MULTIPLIER; 
        velocityLocked = true; 
        curBallRB.linearVelocity = curBallRB.linearVelocity.normalized * velocityLockedSpeed; 

        yield return new WaitForSeconds(DAMPEN_TIME); 

        curBallRB.gravityScale = originalGravity; 
        velocityLocked = false; 
        curBallRB.linearVelocity = curBallRB.linearVelocity.normalized * originalSpeed; 
    }
}
