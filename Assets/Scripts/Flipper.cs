using UnityEngine; 

public class Flipper : MonoBehaviour
{
    [Header("Flipper Settings")]
    public bool isLeftFlipper = true; 
    public float restAngle = -30f; 
    public float activeAngle = 30f; 
    public float flipSpeed = 10f; 

    Rigidbody2D rigidBody;  
    float targetAngle; 
    PinballControls controls; 

    void OnEnable()
    {
        controls?.Enable();          
    }

    void OnDisable()
    {
        controls?.Disable();       
    }

    // Creates a rigidBody and collider for one flipper
    void Awake()
    {
        controls = new PinballControls(); 
        rigidBody = GetComponent<Rigidbody2D>(); 
        targetAngle = restAngle; 
    }

    // Checks for inputs and sets the target angle if input is pressed
    void Update()
    {
    
        // Check which flipper it is and constantly check for input
        bool pressed; 
        if (isLeftFlipper)
        {
            pressed = controls.Game.FlipLeft.IsPressed(); 
        }
        else
        {
            pressed = controls.Game.FlipRight.IsPressed(); 
        }

        //If pressed change the targetAngle
        if (pressed)
        {
            targetAngle = activeAngle; 
        }
        else
        {
            targetAngle = restAngle; 
        }

    }

    // Checks for a change in the angle and updates it
    void FixedUpdate()
    {
        float currentAngle = rigidBody.rotation; 
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, flipSpeed * Time.fixedDeltaTime);        
        rigidBody.MoveRotation(newAngle); 

    }

}
