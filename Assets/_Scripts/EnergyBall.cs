using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.player.GetComponent<PlayerHealth>().TakeDamage(5);
            Destroy(gameObject);
            
        }
    }
}
