using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    [Header("Rarity Weights")]
    public int commonWeight = 75; 
    public int rareWeight = 20; 
    public int legendaryWeight = 5; 

    BallData[] allBalls; 

    // loads every ball from assets/resources/balls
    void Awake()
    {
        allBalls = Resources.LoadAll<BallData>("Balls"); 
    }

    // loads in count random balls from all balls in the directory 
    public List<BallData> GetRandomBalls(int count = 3)
    {
        List<BallData> results = new List<BallData>(); 
        List<BallData> alreadyPicked = new List<BallData>(); 

        for(int i = 0; i < count; i++)
        {
            BallData pick = GetOneWeightedRandomBall(alreadyPicked); 

            if(pick != null)
            {
                results.Add(pick); 
                alreadyPicked.Add(pick); 
            }
        }

        return results; 
    }

    // returns the first ball in the array if for some reason it can't find one 
    private BallData GetOneWeightedRandomBall(List<BallData> exclude)
    {
        List<(BallData ball, int weight)> weighted = new List<(BallData, int)>(); 

        // assigns each ball a weight
        foreach(BallData ball in allBalls)
        {
            // can skip over balls that we want to exclude
            if (exclude.Contains(ball))
            {
                continue; 
            }

            int curWeight = 0;  
            switch (ball.rarity)
            {
                case BallRarity.Common:
                    curWeight = commonWeight; 
                    break;
                case BallRarity.Rare: 
                    curWeight = rareWeight; 
                    break; 
                case BallRarity.Legendary: 
                    curWeight = legendaryWeight; 
                    break; 
                default: 
                    curWeight = 0; 
                    break; 
            }

            weighted.Add((ball, curWeight)); 
        }

        // if nothing, just return 
        if(weighted.Count == 0)
        {
            return null; 
        }

        // pick a random point in the total weight range
        int total = 0; 
        foreach(var entry in weighted)
        {
            total += entry.weight; 
        }

        int roll = Random.Range(0, total); 
        int current = 0; 
        foreach(var entry in weighted)
        {
            current += entry.weight; 
            if(roll < current)
            {
                return entry.ball; 
            }
        }

        return weighted[0].ball; 

    }
}
