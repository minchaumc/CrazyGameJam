using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public NavMeshAgent enemy;
    public List<Transform> destinations;
    public Animator enemyAnimator;
    public Transform player;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float idleTime;
    [SerializeField] private float destionationAmount;

    private bool IsWalking;
    private bool IsChasing;

    private Transform currentDestination;
    private Vector3 dest;

    private int RANDOM_NUMBER;
    private int RANDOM_NUMBER2;
    private int RANDOM_NUMBER3;

    private void Start() {

        IsWalking = true;
        RANDOM_NUMBER = Random.Range(0, destinations.Count);
        currentDestination = destinations[RANDOM_NUMBER];
    }

    private void Update() {
        if (IsWalking == true) {

            dest = currentDestination.position;
            enemy.destination = dest;
            enemy.speed = walkSpeed;

            if (enemy.remainingDistance <= enemy.stoppingDistance) {

                RANDOM_NUMBER2 = Random.Range(0, 2);

                if (RANDOM_NUMBER2 == 0) {

                    RANDOM_NUMBER = Random.Range(0, destinations.Count);
                    currentDestination = destinations[RANDOM_NUMBER];
                }
                if (RANDOM_NUMBER2 == 1) {

                    enemyAnimator.ResetTrigger("walk");
                    enemyAnimator.SetTrigger("idle");

                    StopCoroutine("stayIdle");
                    StartCoroutine("stayIdle");

                    IsWalking = false;
                }
            }
        }
    }

    public IEnumerator stayIdle() {

        idleTime = Random.Range(minIdleTime, maxIdleTime);

        yield return new WaitForSeconds(idleTime);

        IsWalking = true;
        RANDOM_NUMBER = Random.Range(0, destinations.Count);
        currentDestination = destinations[RANDOM_NUMBER];

        enemyAnimator.ResetTrigger("idle");
        enemyAnimator.SetTrigger("walk");
    }
}
