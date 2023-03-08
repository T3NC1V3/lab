using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundlayer, playerlayer;

    // Patrol
    public Vector3 walkPoint;
    bool walkpointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject projectilePos;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        // agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerlayer);

        // Patrol State
        if (!playerInSightRange && !playerInAttackRange) Patrol();
        // Chase State
        if (playerInSightRange && !playerInAttackRange) Chase();
        // Attack State
        if (playerInSightRange && playerInAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!walkpointSet) SearchWalkPoint();

        Debug.Log(walkpointSet);
        if (walkpointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceWalkPoint.magnitude < 1f)
            walkpointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate new point
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 1f, groundlayer))
            walkpointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);

    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody bulletRb = Instantiate(projectile, projectilePos.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            bulletRb.AddForce(transform.forward * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(walkPoint, -transform.up);
    }
}