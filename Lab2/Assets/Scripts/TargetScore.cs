using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using UnityEngine.SocialPlatforms;

public class TargetScore : MonoBehaviour
{
    private Color initialColor;
    private Renderer targetRenderer;
    public float flashDuration = 1f;

    public TextMeshProUGUI scoreText;
    private float totalScore = 0f;

    public ParticleSystem hitParticles;

    public UnityEvent OnHitEvent;
    public Transform throwOrigin;

    void Start()
    {
        //save initial color
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            initialColor = targetRenderer.material.color;
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }
    }
    public void DisplayScore(float hitScore)
    {
        totalScore += hitScore;

        //show score (visually)
        Debug.Log($"Target hit! Total Score: {totalScore:F2} (+{hitScore:F2})");

        StartCoroutine(FlashTarget());

        //update text
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore:F0} (+{hitScore:F0})";
        }
    }

    private IEnumerator FlashTarget()
    {
        targetRenderer.material.color = Color.green;

        yield return new WaitForSeconds(flashDuration);

        targetRenderer.material.color = initialColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        //tag of throwing dart is "Dart"
        if (other.CompareTag("Dart"))
        {
            Vector3 impactPoint = Physics.ClosestPoint(other.transform.position, GetComponent<Collider>(), transform.position, transform.rotation);

            float hitScore = CalculateScore(other.transform.position);
            DisplayScore(hitScore);
            OnHitEvent.Invoke();

            if (hitParticles != null)
            {
                hitParticles.transform.position = impactPoint;

                hitParticles.Play();
            }


            //stop dart from moving through target
            //Rigidbody rb = other.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //rb.isKinematic = true;
            //}

            //destroy dart after some time
            Destroy(other.gameObject, 30f);
        }
    }

    private float CalculateScore(Vector3 impactPosition)
    {
        Vector3 targetCenter = transform.position;


        Vector3 fromCenterToImpact = impactPosition - targetCenter;
        Vector3 projectedImpactOnTargetPlane = Vector3.ProjectOnPlane(fromCenterToImpact, transform.forward);


        float distanceFromCenter = projectedImpactOnTargetPlane.magnitude;

        //maxscore
        float maxScore = 100f;
        float maxTargetRadius = 5f;

        float normalizedDistance = Mathf.Clamp(distanceFromCenter / maxTargetRadius, 0f, 1f);

        //score is inversely proportional to distance
        float proximityFactor = 1.0f - normalizedDistance;

        float finalScore = maxScore * proximityFactor;

        return finalScore;
    }
}