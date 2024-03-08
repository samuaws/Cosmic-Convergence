using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public GameObject mainRig;
    Rigidbody rb;
    BoxCollider mainCollider;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCollider =  rb.GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        GetRigComponenets();
        RagdollOff();

    }
    public Collider[] rigColliders;
    public Rigidbody[] rigRigids;

    public void GetRigComponenets()
    {
        rigColliders = mainRig.GetComponentsInChildren<Collider>();
        rigRigids = mainRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollOff()
    {
       // rb.isKinematic = false;
        mainCollider.enabled = true;
        anim.enabled = true;

        foreach (Collider collider in rigColliders)
        {
            collider.enabled = false;
        }
        foreach(Rigidbody rigid in rigRigids)
        {
            rigid.isKinematic=true;
        }

    }
    public void RagdollOn()
    {
        rb.isKinematic = true;
        mainCollider.enabled = false;
        anim.enabled = false;

        foreach (Collider collider in rigColliders)
        {
            collider.enabled = true;
        }
        foreach(Rigidbody rigid in rigRigids)
        {
            rigid.isKinematic=false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            RagdollOn();
 
        }
    }
}
