using System.Collections.Generic;
using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    [Header("Bumper Prefab")]
    public GameObject bumperPrefab; 

    [Header("Bumper Spawn Bounds")]
    public float spawnYMin = -2.5f; 
    public float spawnYMax = 8f; 
    public float spawnXMin = -15f; 
    public float spawnXMax = 1f; 

    [Header("Other Bumper Settings")]
    public int minBumpers = 1; 
    public int maxBumpers = 8; 
    public float minSpacingBetweenBumpers = 2f; 
    public float bumperMaxSize = 1f; 
    public float bumperMinSize = 0.5f; 

    Transform bumperContainer; 

    // Called by level transitino to generate a new table when increasing in level
    public void GenerateTable(int level)
    {
        bumperContainer = transform.Find("Bumper_Container"); 

        int bumperCount = CalculateBumperCount(level); 
        List<Vector2> placedPositions = new List<Vector2>(); 

        for(int i = 0; i < bumperCount; i++)
        {
            Vector2 position = FindValidPositions(placedPositions); 
            if(position == Vector2.zero)
            {
                break;
            }

            GameObject newBumper = Instantiate(bumperPrefab, bumperContainer); 
            newBumper.transform.localPosition = new Vector3(position.x, position.y, 0); 

            ConfigureBumper(newBumper, level); 
            placedPositions.Add(position); 
        }

    }

    // for now, reduces 1 bumper per level 
    int CalculateBumperCount(int level)
    {
        int count = maxBumpers - (level - 1); 
        count = Mathf.Clamp(count, minBumpers, maxBumpers); 
        return count; 
    }


    // trys a bunch of times to generate a valid spot within the boundaries and not too close to another bumper
    Vector2 FindValidPositions(List<Vector2> placedPositions)
    {
        int maxAttempts = 50; 

        for(int i = 0; i < maxAttempts; i++)
        {
            Vector2 candidate = new Vector2(Random.Range(spawnXMin, spawnXMax), Random.Range(spawnYMin, spawnYMax));       

            if(IsValidPosition(candidate, placedPositions))
            {
                return candidate; 
            }
        }

        // if it couldn't find a valid position
        return Vector2.zero; 
    }


    // this is checking if the current new bumper candidate is within the minDistance of any of the already placed bumpers
    // if it is, it is not a valid spawn position and we must randomize again
    bool IsValidPosition(Vector2 candidate, List<Vector2> placedPositions)
    {
        foreach(Vector2 existing in placedPositions)
        {
            if(Vector2.Distance(candidate, existing) < minSpacingBetweenBumpers)
            {
                return false; 
            }
        }
        return true; 
    }


    // configures the bumper sizes and points awarded depending on the level
    void ConfigureBumper(GameObject newBumper, int level)
    {
        Bumper bumperScript = newBumper.GetComponent<Bumper>(); 

        // higher levels bigger circle size, and update collider to match visuals
        float size = Mathf.Lerp(bumperMaxSize, bumperMinSize, (level - 1) / 10f); 

        // for now, higher levels is slightly more points per hit
        if(bumperScript != null)
        {
            bumperScript.SetSize(size); 
            bumperScript.pointsAwarded = 10f * level; 
        }
    }


}
