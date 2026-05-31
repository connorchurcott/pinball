using UnityEngine;

public class Drain : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ball"))
        {
            return; 
        }

        collision.gameObject.SetActive(false); 
        GameManager.Instance.OnBallDrained(collision.gameObject); 
    }
}
