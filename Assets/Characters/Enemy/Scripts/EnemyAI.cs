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
    [SerializeField] float turnSpeed=5f;

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
        FaceTarget();
        if (navMeshAgent.stoppingDistance<distanceToTarget) {
            ChaseEnemy();
        } else if (navMeshAgent.stoppingDistance>=distanceToTarget) {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
        print("Attacking Target");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawWireSphere(transform.position,chaseRange);
    }

    private void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*turnSpeed);
        
    }
}
