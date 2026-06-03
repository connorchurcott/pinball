using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTransition : MonoBehaviour
{
    [Header("References")]
    public GameObject tablePrefab; 

    [Header("General Transition Settings")]
    public float slideSpeed = 15f; 

    [Header("Slide In Settings")]
    public float slideInStartPos = 22f; 
    public float overshootAmount = 1.5f; 
    public float entryDuration = 0.5f; 
    public float settleDuration = 0.3f; 

    [Header("Slide Out Settings")]
    public float slideOutEndPos = -22f; 
    public float recoilDuration = 0.12f; 
    public float exitDuration = 0.5f; 

    GameObject currentTable; 

    // automatically generate the level 1 table 
    void Start()
    {
        int FIRST_LEVEL = 1;
        currentTable = GameObject.Find("Table"); 

        TableGenerator generator = currentTable.GetComponent<TableGenerator>(); 
        if(generator != null)
        {
            generator.GenerateTable(FIRST_LEVEL); 
        }
    }

    // called by gamemanager to be the definitive table transition
    public void StartTableTransition(int nextLevel, System.Action onComplete)
    {
        StartCoroutine(TransitionRoutine(nextLevel, onComplete)); 
    }

    // StartCoroutine is how you start a function that allows for coroutines, you can't just call that function directly 
    // yield return stops at that line and continues next frame.
    // yield return StartCoroutine starts this corotinue and does not move on until that coroutine is finshed. 
    IEnumerator TransitionRoutine(int nextLevel, System.Action onComplete)
    {
        // beings sliding out current play table, doesn't move on until that is done
        yield return StartCoroutine(SlideOut(currentTable)); 
        Destroy(currentTable); 

        // generate new table after destroying old one to ensure bumpers get placed on new table 
        GameObject newTable = Instantiate(tablePrefab); 
        newTable.transform.position = new Vector3(0, slideInStartPos, 0); 

        TableGenerator generator = newTable.GetComponent<TableGenerator>(); 
        if(generator != null)
        {
            generator.GenerateTable(nextLevel); 
        }
        currentTable = newTable; 

        // begin sliding in new table, doesn't move on until done 
        yield return StartCoroutine(SlideIn(currentTable)); 

        // once done gameManager can continue 
        onComplete?.Invoke(); 
    }

    // does the animation of the current game table sliding down out of the screen
    IEnumerator SlideOut(GameObject table)
    {
        Vector3 startPos = table.transform.position; 
        Vector3 recoilPos = startPos + new Vector3(0, overshootAmount, 0); 
        Vector3 exitPos = new Vector3(0, slideOutEndPos, 0); 

        // recoil up
        float t = 0f; 
        while(t < recoilDuration)
        {
            t += Time.deltaTime; 
            table.transform.position = Vector3.Lerp(startPos, recoilPos, t / recoilDuration); 
            yield return null; 
        }

        // then move down off screen
        t = 0f; 
        while(t < exitDuration)
        {
            t += Time.deltaTime; 
            float easeIn = EaseInCubic(t / exitDuration); 
            table.transform.position = Vector3.Lerp(recoilPos, exitPos, easeIn); 
            yield return null; 
        }

    }

    // does the animation of the next table sliding into the screen 
    IEnumerator SlideIn(GameObject table)
    {
        Vector3 startPos = table.transform.position; 
        Vector3 overshootPos = new Vector3(0,  0 - overshootAmount, 0); 
        Vector3 finalPos = Vector3.zero; 

        // overshoot first
        float t = 0f; 
        while(t < entryDuration)
        {
            t += Time.deltaTime; 
            float easeOut = EaseOutCubic(t / entryDuration); 
            table.transform.position = Vector3.Lerp(startPos, overshootPos, easeOut); 
            yield return null; 
        }

        // settle back into final position
        t = 0f; 
        while(t < settleDuration)
        {
            t += Time.deltaTime; 
            table.transform.position = Vector2.Lerp(overshootPos, finalPos, t / settleDuration); 
            yield return null; 
        }

        table.transform.position = finalPos; 
    }

    float EaseInCubic(float t)
    {
        return t * t * t; 
    }

    float EaseOutCubic(float t)
    {
        float res = 1f - ((1 - t) * (1 - t) * (1 - t));  
        return res; 
    }
}

