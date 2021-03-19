using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] float hitPoints = 100;
        public void TakeDamage(float damage)
        {
            hitPoints -= damage;
            BroadcastMessage("OnDamageTaken");
            if (hitPoints <= 0) { Destroy(gameObject); }
        }
    }

}
