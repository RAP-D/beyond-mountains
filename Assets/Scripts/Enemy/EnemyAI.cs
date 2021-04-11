using Core;
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
        [SerializeField] float visibleRange = 20f;
        [SerializeField] float turnSpeed = 5f;
        [SerializeField] float EnemySightAngle = 90f;
        [SerializeField] float EnemySearchTime = 10f;
        float distanceToTarget = Mathf.Infinity;
        private NavMeshAgent navMeshAgent;
        [SerializeField] private EnemyBehavior enemyBehavior =EnemyBehavior.Guard;
        [SerializeField] private EnemyEvents lastTriggeredEvent=EnemyEvents.Neutral;
        [SerializeField] private PatrolPathController patrolPath;
        private Vector3 lastKnownTargetPosition;
        private Vector3 guardPosition;
        private Quaternion spawnRotation;
        int currentWaypointIndex = 0;
        private float positionTolerance=2f;
        private EnemyGroup enemyGroup;

        enum EnemyBehavior{
            Engage,
            Suspicious,
            SearchForEnemy,
            Guard,
            Back,
            Follow
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
            enemyGroup = GetComponentInParent<EnemyGroup>();
            guardPosition = transform.position;
            spawnRotation=transform.rotation;
        }
        void Update()
        {
            // Don't change the order of these methods. 
            GetEnemyBehaviorOfThisFrame();
            InteractWithTargetInThisFrame();
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
            else if (distanceToTarget <= visibleRange && IsInSight())
            {
                // Trigger if the player moves out of the chase range while enemy engage with him.
                if (enemyGroup.isProvoked && !this.gameObject.Equals(enemyGroup.GetProvokedEnemy()))
                {
                    //Trigger if group provoked 
                    enemyBehavior = EnemyBehavior.Follow;
                }
                else 
                {
                    if (lastTriggeredEvent == EnemyEvents.InChaseRangeAndOnSight)
                    {
                        enemyBehavior = EnemyBehavior.Engage;
                    }
                    // Trigger if the player in the visible range.
                    else if (lastTriggeredEvent == EnemyEvents.InSuspiciousRangeAndOnSight)
                    {
                        // Trigger if the player was search or engage by enemy.
                        if (enemyBehavior == EnemyBehavior.Engage || enemyBehavior == EnemyBehavior.SearchForEnemy || enemyBehavior == EnemyBehavior.Back)
                        {
                            // Continue search.
                            enemyBehavior = EnemyBehavior.SearchForEnemy;
                            lastKnownTargetPosition = target.position;
                        }
                        else
                        {
                            // Trigger if not the player was search or engage by enemy (if player got into visible range from out of it. ).
                            enemyBehavior = EnemyBehavior.Suspicious;
                        }
                    }
                    lastTriggeredEvent = EnemyEvents.InSuspiciousRangeAndOnSight; // Set this frame EnemyEvent to use the next frame.
                }
            }
            else
            {
                // Trigger related events if the player is out of visible range in this frame.
                if (enemyGroup.isProvoked && !this.gameObject.Equals(enemyGroup.GetProvokedEnemy()))
                {
                        //Trigger if group provoked 
                        enemyBehavior = EnemyBehavior.Follow;
                }
                else {
                    if (lastTriggeredEvent == EnemyEvents.OnCollide)
                    {
                        // Trigger the player collide with enemy's back. 
                        enemyBehavior = EnemyBehavior.Engage;
                    }
                    else if (lastTriggeredEvent != EnemyEvents.OutOfRange && (enemyBehavior == EnemyBehavior.SearchForEnemy))
                    {
                        // Trigger if the player moves out of visible range while enemy search for him or Engage with him.
                        enemyBehavior = EnemyBehavior.SearchForEnemy;
                        lastKnownTargetPosition = target.position;
                        lastTriggeredEvent = EnemyEvents.OutOfRange;// Set this frame EnemyEvent to use the next frame.
                    }
                    else if (lastTriggeredEvent != EnemyEvents.OutOfRange && (enemyBehavior == EnemyBehavior.Suspicious|| enemyBehavior==EnemyBehavior.Follow))
                    {
                        // Trigger if the player moves out of visible range while Enemy looks Suspiciously.
                        transform.rotation = spawnRotation;
                        lastTriggeredEvent = EnemyEvents.OutOfRange; // Set this frame EnemyEvent to use the next frame.
                        enemyBehavior = EnemyBehavior.Guard;
                    }
                }
            }
        }
        private void OnCollisionEnter(Collision collision)// Trigger OnCollide event when colliding with the player and set related enemyBehavior.
        {
            if (collision.gameObject.transform.Equals(target.transform))
            {
                enemyBehavior = EnemyBehavior.Engage;
                lastTriggeredEvent = EnemyEvents.OnCollide;
            }
        }
        public void OnDamageTaken()// Trigger OnDamageTaken event when taking damage and set related enemyBehavior.
        {
            if (lastTriggeredEvent == EnemyEvents.InChaseRangeAndOnSight || lastTriggeredEvent == EnemyEvents.InSuspiciousRangeAndOnSight || lastTriggeredEvent == EnemyEvents.OnCollide)
            {
                // Trigger if the player in chase range or suspicious range.
                enemyBehavior = EnemyBehavior.Engage;
            }
            else
            {
                // Trigger if the player out of the suspicious range.
                enemyBehavior = EnemyBehavior.SearchForEnemy;
                lastKnownTargetPosition = target.position;
            }
            lastTriggeredEvent = EnemyEvents.OnDamageTaken; // Set this frame EnemyEvent to use the next frame.
        }
        private void InteractWithTargetInThisFrame()
        {
            // Don't change the order.
            if (enemyBehavior == EnemyBehavior.Suspicious)
            {
                FaceTarget();
            }
            else if (enemyBehavior == EnemyBehavior.SearchForEnemy)
            {
                StartCoroutine(SearchForEnemy());
            }
            else if (enemyBehavior == EnemyBehavior.Back)
            {
                Back();
                enemyGroup.CancelCall();
            }
            else if (enemyBehavior == EnemyBehavior.Engage)
            {
                EngageEnemy();
                enemyGroup.CallGroup(this.gameObject);
            }
            else if (enemyBehavior == EnemyBehavior.Guard)
            {
                Patrol();
            }
            else if (enemyBehavior == EnemyBehavior.Follow)
            {
                MoveEnemyTo(enemyGroup.GetProvokedEnemyPosition());
            }
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
            bool movementDone= MoveEnemyTo(lastKnownTargetPosition);
            if (movementDone) {
                yield return new WaitForSeconds(EnemySearchTime);
                enemyBehavior = EnemyBehavior.Back;
                GetComponent<Animator>().SetTrigger("move");
            }
        }
        private void Back()
        {
            bool movementDone=MoveEnemyTo(guardPosition);
            if (movementDone) {
                transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * turnSpeed);
                enemyBehavior = EnemyBehavior.Guard;
                lastTriggeredEvent = EnemyEvents.Neutral;
            }
        }
        private bool MoveEnemyTo(Vector3 position)
        {
            //TODO review 
            if(enemyBehavior == EnemyBehavior.Back) {
                navMeshAgent.SetDestination(position);
                if (Vector3.Distance(transform.position, position) < positionTolerance)
                {
                    return true; ;
                }
                else { 
                    return false;
                }
            }else
            {
                if (navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    navMeshAgent.SetDestination(navMeshAgent.pathEndPosition);
                    if (Vector3.Distance(transform.position, navMeshAgent.pathEndPosition) < positionTolerance)
                    {
                        return true; ;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.SetDestination(position);
                    if (Vector3.Distance(transform.position, position) < positionTolerance)
                    {
                        return true; ;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
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
        private void Patrol()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath!=null) {
                if (AtWaypoint()) {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            MoveEnemyTo(nextPosition);
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint <positionTolerance;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visibleRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}

