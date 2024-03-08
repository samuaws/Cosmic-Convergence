using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public float pullRadius = 10f; // Radius within which enemies are pulled towards the blackhole
    public float pullForce = 10f; // Force with which enemies are pulled towards the blackhole
    public float duration = 5f; // Duration of the blackhole effect

    private float timer; // Timer to track the duration of the blackhole effect

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject); // Destroy the blackhole after the specified duration
        }
    }

    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calculate direction from target to blackhole
                    Vector3 direction = transform.position - col.transform.position;

                    // Apply force towards the blackhole
                    rb.AddForce(direction.normalized * pullForce * Time.fixedDeltaTime);
                }
            }
        }
    }
}
