using UnityEngine;
using UnityEngine.Events;

public class TargetScore : MonoBehaviour
{
    public UnityEvent OnHitEvent;
    public Transform throwOrigin; 

    public void DisplayScore(float score)
    {
       
        Debug.Log("Target hit! Score: " + score.ToString("F2")); 
        GetComponent<Renderer>().material.color = Color.yellow; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dart")) 
        {
            float score = CalculateScore(other.transform.position);
            DisplayScore(score);
            OnHitEvent.Invoke(); 
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; 
            }
            Destroy(other.gameObject, 5f);
        }
    }

    private float CalculateScore(Vector3 impactPosition)
    {
        if (throwOrigin == null)
        {
            Debug.LogError("Throw Origin not set in Inspector!");
            return 0f;
        }

        float distanceToTarget = Vector3.Distance(throwOrigin.position, transform.position);
        float baseScore = distanceToTarget * 10f; 
        return baseScore;
    }
}
