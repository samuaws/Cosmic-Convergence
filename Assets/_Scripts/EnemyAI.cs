using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform target; // Target to chase (usually the player)
    public float chaseRange = 10f; // Range within which the enemy will chase the target
    public float attackRange = 2f; // Range within which the enemy will attack the target
    public float movementSpeed = 5f; // Speed at which the enemy moves
    public float rotationSpeed = 5f; // Speed at which the enemy rotates towards the target

    public Transform childToMatch;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool dead = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameManager.Instance.player.transform;
    }

    void Update()
    {
        if (dead || !navMeshAgent.enabled) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Chase the target if within chase range
        if (distanceToTarget <= chaseRange)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetBool("isWalking", true);

            // Rotate towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Attack if within attack range
            if (distanceToTarget <= attackRange)
            {
                // Perform attack
                animator.SetBool("attack", true);
                navMeshAgent.isStopped = true;
                // Add attack logic here
            }
            else
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("isWalking", true);
                animator.SetBool("attack", false);
            }
        }
        else
        {
            // Stop moving if target is out of range
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("attack", false);
        }
    }
    public void StopFollow()
    {
        if(navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
            animator.SetBool("isWalking", false);
        }
    }
    public void ContinueFollow()
    {
        if(navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
    }
    public void GetUp()
    {
        StartCoroutine(GetUpCouroutine());
    }
    private void ResetParentPosition()
    {
        
        Vector3 offset = (new Vector3(childToMatch.position.x, 0, childToMatch.position.z) - transform.position) ; 
        transform.position = new Vector3(childToMatch.position.x, 0, childToMatch.position.z);
        foreach (Transform child in transform)
        {
            child.position -= offset;
        }
    }
    private IEnumerator GetUpCouroutine()
    {
        print("Getting up");
        yield return new WaitForSeconds(2);
        GetComponent<RagdollOnOff>().RagdollOff();
        ResetParentPosition();
        animator.Play("GettingUp",-1,0);
        animator.enabled = true;
        yield return new WaitForSeconds(7.3f);
        ContinueFollow();
    }

    public void Die()
    {
        dead = true;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 2f); 
    }
}
