using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NEScaco : MonoBehaviour
{
    [SerializeField] int health;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttackedQ;
    public GameObject projectile;


    //states
    public float SightRange, attackRange;
    //[SerializeField] 
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cheack for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, SightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, SightRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patrolng();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    void Patrolng()
    {
        if (!walkPointSet) SearchWalkpoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        Vector3 distancetoWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distancetoWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkpoint()
    {
        //calculate random point in range
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttackedQ)
        {
            //attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 32f, ForceMode.Impulse);

            ///

            alreadyAttackedQ = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttackedQ = false;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestoryEnemy), .5f);
    }

    void DestoryEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}
