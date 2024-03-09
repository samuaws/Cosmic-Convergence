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

    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider col in colliders)
        {
            colidersToPull.Add(col);
        }
        foreach (Collider col in colidersToPull)
        {
            if (col.transform.root.CompareTag("Enemy"))
            {

                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.transform.root.GetComponent<Rigidbody>().isKinematic = false;
                    rb.transform.root.GetComponent<EnemyAI>().StopFollow();
                    rb.transform.root.GetComponent<EnemyAI>().StopAllCoroutines();
                    rb.transform.root.GetComponent<RagdollOnOff>().RagdollOn();

                        Vector3 direction = transform.position - col.transform.position;
                        rb.AddForce(direction.normalized * pullForce);

                    // Apply force towards the blackhole
                }
            }
        }
    }
    private void OnDestroy()
    {

        HashSet<Transform> processedRoots = new HashSet<Transform>();

        foreach (Collider col in colidersToPull)
        {
            Transform root = col.transform.root;
            if (!processedRoots.Contains(root))
            {
                processedRoots.Add(root);

                if (root.CompareTag("Enemy"))
                {
                   root.GetComponent<EnemyAI>().GetUp();  
                }
            }
        }
    }
}
