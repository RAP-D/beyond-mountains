using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] float attackDamage=30;
    public void AttackHitEvent() {
        if (target==null) { return; }
        print("Attacking");
    }
}
