using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerHealth target;
    [SerializeField] float attackDamage=30;

    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }
    public void AttackHitEvent() {
        if (target==null) { return; }
        target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }
}
