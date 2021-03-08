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
    void Start()
    {
        navMeshAgent=GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position,transform.position);
        if (distanceToTarget<=chaseRange) { navMeshAgent.SetDestination(target.position); }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawWireSphere(transform.position,chaseRange);
    }
}
