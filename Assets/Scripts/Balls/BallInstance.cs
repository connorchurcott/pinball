using UnityEngine;

public class BallInstance : MonoBehaviour
{
    public BallData data; 

    float cooldownRemaining = 0f; 
    public bool IsNormalBall { get; private set; }  

    PinballControls controls; 

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
        if(data == null || data.ability == BallAbility.None )
        {
            IsNormalBall = true; 
        }

        if(data != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>(); 
            if(sr != null)
            {
                sr.color = data.ballColor; 
            }
        }       
    }

    void Update()
    {
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

    void ActivateAbility()
    {

        // hard coded for SpawnOneMultiball, in future i gotta add functions for each ability and a enum or smthn 
        if(data.ability == BallAbility.SpawnOneMultiball)
        {
            GameManager.Instance.SpawnNormalBall(transform.position); 
            cooldownRemaining = data.cooldown; 
            return; 
        }
    }

}
