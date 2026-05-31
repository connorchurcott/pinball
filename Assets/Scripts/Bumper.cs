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


    [Header("Events")]
    public UnityEvent<float> bumperHit; 

    Transform visual;
    Vector3 originalScale; 

    void Awake()
    {
        visual = transform.Find("Visual"); 
        originalScale = visual.localScale; 
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
            bumperHit.Invoke(score); 
        }

        StopAllCoroutines(); 
        visual.localScale = originalScale; 
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
            visual.localScale = Vector3.Lerp(originalScale, expandedScale, t / expandDuration); 
            yield return null; 
        }

        // scale DOWN 
        t = 0f; 
        while( t < shrinkDuration)
        {
            t += Time.deltaTime; 
            visual.localScale = Vector3.Lerp(expandedScale, originalScale, t / shrinkDuration); 
            yield return null; 
        }

        visual.localScale = originalScale; 
    }
}
