using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Blackhole : MonoBehaviour
{
    public float pullRadius = 10f; // Radius within which enemies are pulled towards the blackhole
    public float pullForce = 10f; // Force with which enemies are pulled towards the blackhole
    public float duration = 5f; // Duration of the blackhole effect
    private HashSet<Collider> colidersToPull = new HashSet<Collider>();

    private float timer; // Timer to track the duration of the blackhole effect

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
           // Destroy(gameObject); // Destroy the blackhole after the specified duration
        }
    }

    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider col in colliders)
        {
            colidersToPull.Add(col);
        }
        print(colidersToPull.Count);
        foreach (Collider col in colidersToPull)
        {
            if (col.transform.root.CompareTag("Enemy"))
            {
            print(col.gameObject.name);
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.transform.root.GetComponent<Rigidbody>().isKinematic = false;
                    rb.transform.root.GetComponent<EnemyAI>().StopFollow();
                    rb.transform.root.GetComponent<RagdollOnOff>().RagdollOn();

                        Vector3 direction = transform.position - col.transform.position;
                        rb.AddForce(direction.normalized * pullForce);

                    // Apply force towards the blackhole
                }
            }
        }
    }
}
