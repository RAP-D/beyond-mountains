using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyGroup : MonoBehaviour
    {
        public bool isProvoked=false;
        [SerializeField]private GameObject provokedEnemy;
        public void CallGroup(GameObject enemy)
        {
            isProvoked = true;
            provokedEnemy = enemy;
        }
        public void CancelCall()
        {
            isProvoked = false;
        }

        public Vector3 GetProvokedEnemyPosition() {
            return provokedEnemy.transform.position;
        }
        public GameObject GetProvokedEnemy()
        {
            return provokedEnemy;
        }
    }
}

