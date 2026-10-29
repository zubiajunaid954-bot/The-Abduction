using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator anim;
    public Transform[] patrolPoints;
    public Transform player;
    public Transform firePoint; // The gun muzzle
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float detectionRange = 15f;
    public float shootingRange = 10f;
    public float fireRate = 1f;
    public float patrolWaitTime = 2f;

    private int currentPatrolIndex = 0;
    private float fireTimer = 0f;
    private bool isPlayerDetected = false;

    void Start()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        fireTimer += Time.deltaTime;

        if (distance <= detectionRange)
        {
            isPlayerDetected = true;
            agent.isStopped = true;
            transform.LookAt(player);

            anim.SetBool("isWalking", false);
            anim.SetBool("isAiming", true);

            if (distance <= shootingRange && fireTimer >= 1f / fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            if (isPlayerDetected)
            {
                // Lost player, resume patrol
                isPlayerDetected = false;
                agent.isStopped = false;
            }
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("isAiming", false);
        anim.SetBool("isWalking", true);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndNextPoint());
        }
    }

    System.Collections.IEnumerator WaitAndNextPoint()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(patrolWaitTime);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        agent.isStopped = false;
    }

    void Shoot()
    {
        anim.SetBool("isShooting", true);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        anim.SetBool("isShooting", false);
    }
}
