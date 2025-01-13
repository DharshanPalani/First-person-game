using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling properties
    [Header("Patrolling Settings")]
    public float walkPointRange = 10f;
    private Vector3 walkPoint;
    private bool walkPointSet;

    // Attack & Detection ranges
    [Header("Detection & Attack Ranges")]
    public float sightRange, attackRange;

    // Timers and Flags
    [Header("Attack Settings")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    private bool PlayerInSightRange => Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    private bool PlayerInAttackRange => Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check player distance and act accordingly
        if (!PlayerInSightRange && !PlayerInAttackRange) Patrol();
        else if (PlayerInSightRange && !PlayerInAttackRange) Chase();
        else if (PlayerInSightRange && PlayerInAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SetWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Debug.Log("Patrolling towards: " + walkPoint);

            // Check if enemy reached patrol point
            if (Vector3.Distance(transform.position, walkPoint) < 1f)
            {
                walkPointSet = false;
                Debug.Log("Reached patrol point, searching for new walk point.");
            }
        }
    }

    private void SetWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            Debug.Log("New walk point set: " + walkPoint);
        }
        else
        {
            Debug.Log("Walk point is not valid, trying again.");
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
        Debug.Log("Chasing player: " + player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position); // Stop movement
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            int damage = 10;
            
            if (player.GetComponent<Health>())
            {
                player.GetComponent<Health>().UpdateHealth(-damage);
            }


            var cameraShake = FindObjectOfType<CameraShake>();
            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.1f, 0.1f));
            }

            alreadyAttacked = true;
            Debug.Log("Attacking player...");

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        Debug.Log("Attack cooldown over, ready to attack again.");
    }
}
