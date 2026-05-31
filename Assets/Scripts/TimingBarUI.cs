using UnityEngine;

public class TimingBarUI : MonoBehaviour
{
    [Header("References")]   
    public RectTransform indicator; 
    public RectTransform greenZone; 
    public BallLauncher launcher; 

    [Header("Settings")]
    public float barHeight = 400f; 

    void Awake()
    {
        // position greenZone bassed on launcher values
        float greenCenter = launcher.GetGreenZoneCenter(); 
        float greenSize = launcher.GetGreenZoneSize(); 

        float greenY = (greenCenter * barHeight) + (greenSize * barHeight / 2f); 
        greenZone.anchoredPosition = new Vector2(0, greenY); 
        greenZone.sizeDelta = new Vector2(50, greenSize * barHeight); 
    }

    void Update()
    {
        // move the indicator bar 
        float barPosition = launcher.GetBarPosition(); 
        float indicatorY = barPosition * barHeight; 
        indicator.anchoredPosition = new Vector2(0, indicatorY); 
    }

}
