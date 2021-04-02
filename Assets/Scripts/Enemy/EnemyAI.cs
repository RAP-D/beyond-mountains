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
        [SerializeField] float suspiciousRange = 20f;
        [SerializeField] float turnSpeed = 5f;
        [SerializeField] float EnemySightAngle = 90f;
        [SerializeField] float EnemySearchTime = 10f;
        float distanceToTarget = Mathf.Infinity;
        NavMeshAgent navMeshAgent;
        [SerializeField] private EnemyBehavior enemyBehavior =EnemyBehavior.Neutral;
        [SerializeField] private EnemyEvents lastTriggeredEvent=EnemyEvents.Neutral;
        Vector3 lastKnownTargetPosition;
        Vector3 spawnPosition;
        private Quaternion spawnRotation;
        
        enum EnemyBehavior{
            Engage,
            LookSuspicious,
            SearchForEnemy,
            Neutral
        }
        enum EnemyEvents {
            InChaseRangeAndOnSight,
            InSuspiciousRangeAndOnSight,
            OutOfRange,
            OnDamageTaken,
            OnCollide,
            Neutral
        }


        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            spawnPosition = transform.position;
            spawnRotation=transform.rotation;
        }

        void Update()
        {
            // Don't change the order of these methods. 
            GetEnemyBehaviorOfThisFrame();
            InteractWithTargetInThisFrame();
        }

        private void InteractWithTargetInThisFrame()
        {

            // Don't change the order.
            if (enemyBehavior  == EnemyBehavior.LookSuspicious) {
                FaceTarget();
            } else if (enemyBehavior  == EnemyBehavior.SearchForEnemy) {
                StartCoroutine(SearchForEnemy());
            } else if (enemyBehavior  == EnemyBehavior.Engage) {
                EngageEnemy();
            }
        }

        private void GetEnemyBehaviorOfThisFrame()
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            if (distanceToTarget <= chaseRange && IsInSight())
            {
                // Trigger if the player is in chase range and on Enemy's Sight.
                enemyBehavior = EnemyBehavior.Engage;
                lastTriggeredEvent = EnemyEvents.InChaseRangeAndOnSight; // Set this frame EnemyEvent to use the next frame.
            }
            else if (distanceToTarget <= suspiciousRange && IsInSight())
            {
                // Trigger if the player moves out of the chase range while enemy engage with him.
                if (lastTriggeredEvent == EnemyEvents.InChaseRangeAndOnSight)
                {
                    enemyBehavior  = EnemyBehavior.Engage;
                }
                // Trigger if the player in the suspicious range.
                else if (lastTriggeredEvent == EnemyEvents.InSuspiciousRangeAndOnSight)
                {
                    // Trigger if the player was search or engage by enemy.
                    if (enemyBehavior  == EnemyBehavior.Engage || enemyBehavior  == EnemyBehavior.SearchForEnemy)
                    {
                        // Continue search.
                        enemyBehavior  = EnemyBehavior.SearchForEnemy;
                        lastKnownTargetPosition = target.position;
                    }
                    else
                    {
                        // Trigger if not the player was search or engage by enemy (if player got into suspicious range from out of it. ).
                        enemyBehavior = EnemyBehavior.LookSuspicious;
                    }
                }
                lastTriggeredEvent = EnemyEvents.InSuspiciousRangeAndOnSight; // Set this frame EnemyEvent to use the next frame.
            }
            else
            {
                // Trigger related events if the player is out of suspicious range in this frame.
                if (lastTriggeredEvent == EnemyEvents.OnCollide)
                {
                    // Trigger the player collide with enemy's back. 
                    enemyBehavior = EnemyBehavior.Engage;
                }
                else if (lastTriggeredEvent != EnemyEvents.OutOfRange && (enemyBehavior  == EnemyBehavior.SearchForEnemy || enemyBehavior  == EnemyBehavior.Engage))
                {
                    // Trigger if the player moves out of suspicious range while enemy search for him or Engage with him.
                    enemyBehavior = EnemyBehavior.SearchForEnemy;
                    lastKnownTargetPosition = target.position;
                    lastTriggeredEvent = EnemyEvents.OutOfRange;// Set this frame EnemyEvent to use the next frame.
                }
                else if (lastTriggeredEvent != EnemyEvents.OutOfRange && enemyBehavior  == EnemyBehavior.LookSuspicious)
                {
                    // Trigger if the player moves out of suspicious range while Enemy looks Suspiciously.
                    transform.rotation = spawnRotation;
                    lastTriggeredEvent = EnemyEvents.OutOfRange; // Set this frame EnemyEvent to use the next frame.
                    enemyBehavior  = EnemyBehavior.Neutral;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)// Trigger OnCollide event when colliding with the player and set related enemyBehavior.
        {
            enemyBehavior  = EnemyBehavior.Engage;
            lastTriggeredEvent = EnemyEvents.OnCollide;
        }

        public void OnDamageTaken()// Trigger OnDamageTaken event when taking damage and set related enemyBehavior.
        {
            if (lastTriggeredEvent == EnemyEvents.InChaseRangeAndOnSight || lastTriggeredEvent == EnemyEvents.InSuspiciousRangeAndOnSight || lastTriggeredEvent == EnemyEvents.OnCollide)
            {
                // trigger if the player in chase range or suspicious range.
                enemyBehavior  = EnemyBehavior.Engage;
            }
            else
            {
                // trigger if the player out of the suspicious range.
                enemyBehavior = EnemyBehavior.SearchForEnemy;
                lastKnownTargetPosition = target.position;
            }
            lastTriggeredEvent = EnemyEvents.OnDamageTaken; // Set this frame EnemyEvent to use the next frame.
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
            return false;
        }

        private IEnumerator SearchForEnemy()
        {
            navMeshAgent.SetDestination(lastKnownTargetPosition);
            yield return new WaitForSeconds(EnemySearchTime);
            navMeshAgent.SetDestination(spawnPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * turnSpeed);
            enemyBehavior = EnemyBehavior.Neutral;
            lastTriggeredEvent = EnemyEvents.Neutral;
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
        private void ChaseEnemy()
        {
            GetComponent<Animator>().SetBool("attack", false);
            GetComponent<Animator>().SetTrigger("move");
            navMeshAgent.SetDestination(target.position);
        }

        private void AttackTarget()
        {
            GetComponent<Animator>().SetBool("attack", true);
        }

        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, suspiciousRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }       
        

    }
}

