using UnityEngine;
using UnityEngine.WSA;

public class BallLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float normalLaunchForce = 20f; 
    public float perfectLaunchForce = 30f; 

    [Header("Timing Bar Settings")]
    public float barSpeed = 2f; 
    public float greenZoneCenter = 0.5f; 
    public float greenZoneSize = 0.15f; 


    float barPosition = 0f; 
    float barDirection = 1f; 
    //bool isLaunching = false; 
    bool ballInLane = true; 
    Rigidbody2D ball; 

    PinballControls controls;

    void OnEnable()
    {
        controls?.Enable(); 
    }

    void OnDisable()
    {
        controls?.Disable(); 
    }

    void Awake()
    {
        controls = new PinballControls(); 
    }

    void Update()
    {
        if (!ballInLane)
        {
            return; 
        }

        // check bar direction, switch if at either pole
        barPosition += barDirection * barSpeed * Time.deltaTime; 
        if(barPosition >= 1f || barPosition <= 0f)
        {
            barDirection = -barDirection; 
        }

        // check launch input
        if (controls.Game.Launch.WasPressedThisFrame())
        {
            Launch(); 
        }
    }

    void Launch()
    {
        ballInLane = false; 

        // check if inside green zone, set force accordingly 
        bool isPerfect = Mathf.Abs(barPosition - greenZoneCenter) <= greenZoneSize / 2; 
        float force;
        if (isPerfect)
        {
            force = perfectLaunchForce; 
        }
        else
        {
            force = normalLaunchForce; 
        }

        ball.bodyType = RigidbodyType2D.Dynamic; 
        ball.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        // debug 
        if (isPerfect)
        {
            Debug.Log("PERFECT LAUNCH");
        }
        else
        {
            Debug.Log("NORMAL LAUNCH"); 
        }

        //Debug.Log("barPos: " + barPosition + " center: " + greenZoneCenter + " halfSize: " + (greenZoneSize / 2f) + " diff: " + Mathf.Abs(barPosition - greenZoneCenter));
    }

    // This is used when spawning a new ball and not for when the ball reenters the launcher
    public void SetBall(Rigidbody2D newBallRB)
    {
        ball = newBallRB; 
        ballInLane = true; 
        barPosition = 0f; 
    }

    // Use the components boxcollider2d to check if a ball is there, if so just set in lane to true so that the update checks again
    // This is only used when the ball re-enters the launcher, not for spawning new balls
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ball"))
        {
            return; 
        }

        ballInLane = true; 
    }

    public float GetBarPosition()
    {
        return barPosition; 
    }

    public float GetGreenZoneCenter()
    {
        return greenZoneCenter; 
    }

    public float GetGreenZoneSize()
    {
        return greenZoneSize; 
    }

}

