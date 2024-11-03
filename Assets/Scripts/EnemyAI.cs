using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    [SerializeField] private float sightDistance;
    [SerializeField] private float catchDistance;

    [SerializeField] private float minChaseTime;
    [SerializeField] private float maxChaseTime;
    [SerializeField] private float chaseTime;

    [SerializeField] private float jumpscareTime;

    [SerializeField] private string deathScene;

    private bool IsWalking;
    private bool IsChasing;

    private Transform currentDestination;
    private Vector3 dest;
    private Vector3 rayCastOffSet;

    private int RANDOM_NUMBER;
    private int RANDOM_NUMBER2;
    private int RANDOM_NUMBER3;

    private void Start() {

        IsWalking = true;
        RANDOM_NUMBER = Random.Range(0, destinations.Count);
        currentDestination = destinations[RANDOM_NUMBER];
    }

    private void Update() {

        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + rayCastOffSet, direction, out hit, sightDistance)) {
            if (hit.collider.gameObject.CompareTag("Player")) {

                IsWalking = false;

                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");

                enemyAnimator.ResetTrigger("walk");
                enemyAnimator.ResetTrigger("idle");
                enemyAnimator.SetTrigger("sprint");

                IsChasing = true;
            }
        }

        if (IsChasing == true) {
            dest = player.position;
            enemy.destination = dest;
            enemy.speed = chaseSpeed;

            if (enemy.remainingDistance <= catchDistance) {

                player.gameObject.SetActive(false);

                enemyAnimator.ResetTrigger("sprint");
                enemyAnimator.SetTrigger("jumpscare");

                StartCoroutine((IEnumerator)deathRoutine());
                IsChasing = false;
            }

        }

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

    public IEnumerable chaseRoutine() {

        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        
        yield return new WaitForSeconds(chaseTime);

        IsWalking = true;
        IsChasing = false;

        RANDOM_NUMBER = Random.Range(0, destinations.Count);
        currentDestination = destinations[RANDOM_NUMBER];

        enemyAnimator.ResetTrigger("sprint");
        enemyAnimator.SetTrigger("walk");
    }

    public IEnumerable deathRoutine() {

        yield return new WaitForSeconds(jumpscareTime);

        SceneManager.LoadScene(deathScene);
    }
}
