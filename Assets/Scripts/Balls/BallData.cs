using UnityEngine;

public enum BallRarity { Common, Rare, Legendary }

[CreateAssetMenu(fileName = "NewBallData", menuName = "Pinball/BallData")]
public class BallData : ScriptableObject
{
    [Header("Identity")]
    public string ballName = "New Ball"; 
    public string description = "No Ability."; 
    public Sprite ballSprite;
    public Color ballColor = Color.cyan; 
    public BallRarity rarity = BallRarity.Common; 

    [Header("Ability")]
    public BallAbility ability = BallAbility.None;  
    public AbilityActivation activation = AbilityActivation.Passive; 
    public float cooldown = 30f; 
    public float abilityValue = 1f; 
}
