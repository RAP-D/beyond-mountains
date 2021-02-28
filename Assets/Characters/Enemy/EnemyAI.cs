using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent navMeshAgent;
    [SerializeField] GameObject target; 
    void Start()
    {
        navMeshAgent=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
