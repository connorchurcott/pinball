using UnityEngine;

[CreateAssetMenu(fileName = "NewBallData", menuName = "Pinball/BallData")]
public class BallData : ScriptableObject
{
    [Header("Identity")]
    public string ballName = "New Ball"; 
    public Color ballColor = Color.cyan; 

    [Header("Ability")]
    public BallAbility ability = BallAbility.None;  
    public AbilityActivation activation = AbilityActivation.Passive; 
    public float cooldown = 30f; 
    public float abilityValue = 1f; 
}
