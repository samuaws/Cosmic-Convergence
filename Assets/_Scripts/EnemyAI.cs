using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform target; // Target to chase (usually the player)
    public float chaseRange = 10f; // Range within which the enemy will chase the target
    public float attackRange = 2f; // Range within which the enemy will attack the target
    public float movementSpeed = 5f; // Speed at which the enemy moves
    public float rotationSpeed = 5f; // Speed at which the enemy rotates towards the target

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
        if (dead) return;

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

    public void Die()
    {
        dead = true;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 2f); 
    }
}
