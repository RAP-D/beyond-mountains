using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange=5f;
    float distanceToTarget = Mathf.Infinity;
    NavMeshAgent navMeshAgent;
    bool isProvoked = false;
    void Start()
    {
        navMeshAgent=GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked) {
            EngageEnemy();
        } else if (distanceToTarget<=chaseRange) {
            isProvoked = true;
        }
    }

    private void ChaseEnemy()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void EngageEnemy()
    {
        if (navMeshAgent.stoppingDistance<distanceToTarget) {
            ChaseEnemy();
        } else if (navMeshAgent.stoppingDistance>=distanceToTarget) {
            AttactTarget();
        }
    }

    private void AttactTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawWireSphere(transform.position,chaseRange);
    }
}
