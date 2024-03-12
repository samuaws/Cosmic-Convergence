using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform target; // Target to chase (usually the player)
    public float attackRange = 2f; // Range within which the enemy will attack the target
    public float rangedAttackRange = 10f; // Range within which the enemy will attack the target from range
    public float movementSpeed = 5f; // Speed at which the enemy moves
    public float rotationSpeed = 5f; // Speed at which the enemy rotates towards the target
    public int damage = 5;

    public GameObject energyBallPrefab; // Prefab of the energy ball
    public Transform energyBallSpawnPoint; // Point from which the energy balls will be spawned
    public bool performRangedAttack = false; // Whether to perform ranged attacks

    public Transform childToMatch;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float distanceToTarget;
    private bool dead = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameManager.Instance.player.transform;
        navMeshAgent.speed = movementSpeed;
    }

    void Update()
    {
        if (dead || !navMeshAgent.enabled) return;

        distanceToTarget = Vector3.Distance(transform.position, target.position);

        navMeshAgent.SetDestination(target.position);
        animator.SetBool("isWalking", true);

        // Rotate towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Attack based on attack type and distance
        if (performRangedAttack && distanceToTarget <= rangedAttackRange)
        {
            // Perform ranged attack
            animator.SetBool("rangedAttack", true);
            //ShootEnergyBall();
            navMeshAgent.isStopped = true;
        }
        else if (!performRangedAttack && distanceToTarget <= attackRange)
        {
            // Perform melee attack
            animator.SetBool("attack", true);
            navMeshAgent.isStopped = true;
        }
    }
    public void FinishAttack()
    {
        if (!(performRangedAttack && distanceToTarget <= rangedAttackRange) && !(!performRangedAttack && distanceToTarget <= attackRange))
        {
            navMeshAgent.isStopped = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("attack", false);
            animator.SetBool("rangedAttack", false);
        }
    }
    public void ShootEnergyBall()
    {
        // Instantiate the energy ball prefab at the spawn point
        GameObject energyBall = Instantiate(energyBallPrefab, energyBallSpawnPoint.position, Quaternion.identity);
        // Calculate the direction towards the player
        Vector3 direction = (target.position + new Vector3(0,1,0) - energyBall.transform.position).normalized;

        Rigidbody rb = energyBall.GetComponent<Rigidbody>();
        rb.useGravity = false;
        // Set the velocity of the energy ball to shoot towards the player
        rb.AddForce(direction * 30f, ForceMode.VelocityChange); // Adjust the speed as needed
        // Destroy the energy ball after a certain time to prevent cluttering the scene
        Destroy(energyBall, 4f); // Adjust the time as needed
    }
    public void DealDamage()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            GameManager.Instance.player.GetComponent<PlayerHealth>().TakeDamage(damage);
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
        WaveManager.instance.RemoveEnemy(gameObject);
    }
}
