using System.Collections; 
using UnityEngine;
using UnityEngine.Events; 

public class Bumper : MonoBehaviour
{

    [Header("Bumper Settings")]
    public float pointsAwarded = 10; 
    public float force = 2; 

    [Header("Animation")]
    public float expandScale = 1f; 
    public float expandDuration = 0.05f; 
    public float shrinkDuration = 1f; 

    Transform visual;
    Vector3 originalScale; 

    void Awake()
    {
        visual = transform.Find("Visual"); 
        originalScale = transform.localScale; 
    }

    // called by TableTransitions when generating random bumper placements. ensures the scaling is set correctly
    public void SetSize(float size)
    {
        if(visual != null)
        {
            transform.localScale = new Vector3(size, size, 1f); 
            originalScale = transform.localScale; 
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ball"))
        {
            return; 
        }

        // Pointing from bumper to center of ball
        Vector2 direction = (collision.transform.position - transform.position).normalized; 

        // apply force, save incoming speed for point calculation
        Rigidbody2D ballRB = collision.GetComponent<Rigidbody2D>(); 
        if(ballRB != null)
        {
            float curSpeed = ballRB.linearVelocity.magnitude; 
            float launchSpeed = Mathf.Max(curSpeed, force); 
            ballRB.linearVelocity = direction * launchSpeed;

            //active event with the calculated score passed in 
            float score = pointsAwarded * curSpeed; 

            // no more event, just call incrementscore from gamemanager
            GameManager.Instance.IncrementScore(score); 
        }

        StopAllCoroutines(); 
        transform.localScale = originalScale; 
        StartCoroutine(ExpandAnimation()); 
    }

    IEnumerator ExpandAnimation()
    {
        Vector3 expandedScale = originalScale * expandScale; 

        // Scale UP 
        float t = 0f; 
        while( t < expandDuration)
        {
            t += Time.deltaTime; 
            transform.localScale = Vector3.Lerp(originalScale, expandedScale, t / expandDuration); 
            yield return null; 
        }

        // scale DOWN 
        t = 0f; 
        while( t < shrinkDuration)
        {
            t += Time.deltaTime; 
            transform.localScale = Vector3.Lerp(expandedScale, originalScale, t / shrinkDuration); 
            yield return null; 
        }

        transform.localScale = originalScale; 
    }
}
