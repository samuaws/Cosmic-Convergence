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
            if (hit.CompareTag("Enemy"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.gameObject.GetComponent<RagdollOnOff>().RagdollOn();
                    foreach (Rigidbody rigid in rb.gameObject.GetComponent<RagdollOnOff>().rigRigids)
                    {
                        rigid.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                   // Destroy(rb.gameObject, 2);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
