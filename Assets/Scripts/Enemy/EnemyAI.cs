using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float chaseRange = 5f;
        float distanceToTarget = Mathf.Infinity;
        NavMeshAgent navMeshAgent;
        bool isProvoked = false;
        [SerializeField] float turnSpeed = 5f;
        [SerializeField] float EnemySightAngle = 90f;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            
        }
        void Update()
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            if (isProvoked)
            {
                EngageEnemy();
            }
            else if (distanceToTarget <= chaseRange && IsInSight() )
            {
                isProvoked = true;
            }

            IsInSight();
        }

        private bool IsInSight()
        {
            Vector3 direction = (target.position-transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if (Mathf.Abs(angle) < EnemySightAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit)){
                    if (hit.transform.Equals(target))
                    {
                        return true;
                    }
                }
            }
            ;
            return false;
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
            if (navMeshAgent.stoppingDistance < distanceToTarget)
            {
                ChaseEnemy();
            }
            else if (navMeshAgent.stoppingDistance >= distanceToTarget)
            {
                AttackTarget();
            }
        }

        private void AttackTarget()
        {
            GetComponent<Animator>().SetBool("attack", true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
        private void OnCollisionEnter(Collision collision)
        {
            isProvoked = true;
        }
        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        }

        public void OnDamageTaken() { isProvoked = true; }

    }
}

