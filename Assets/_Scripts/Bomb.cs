using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            HashSet<Transform> processedRoots = new HashSet<Transform>();
            if (hit.CompareTag("Enemy"))
            {
                Transform root = hit.transform.root;
                if (!processedRoots.Contains(root))
                {
                    processedRoots.Add(root);

                    if (root.CompareTag("Enemy"))
                    {
                        DamageEnemy(root.GetComponent<Collider>());
                    }
                }
            }
        }
    }
    void DamageEnemy(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gameObject.GetComponent<RagdollOnOff>().RagdollOn();
            foreach (Rigidbody rigid in rb.gameObject.GetComponent<RagdollOnOff>().rigRigids)
            {
                rigid.gameObject.tag = "Enemy";
                if (rigid.transform.parent)
                {
                    rigid.AddExplosionForce(explosionForce/11f, transform.position, explosionRadius);
                }
                else
                {
                    rigid.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
                
            }
            rb.gameObject.GetComponent<EnemyAI>().Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
