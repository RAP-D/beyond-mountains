using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons {
    public class MeleeWeapon : Weapon
    {
        public Camera fpsCamera;
        private bool canAttack;
        private bool isAutomatic;
        Animator animator;
        [SerializeField] GameObject hitImpact;
        [SerializeField] float primaryAttackDamage = 30;
        [SerializeField] float secondaryAttackDamage = 50;
        private float damage;

        private void OnEnable()
        {
            canAttack = true;
        }
        public override void AlternativeAttack()
        {
            if (canAttack) {
                StartCoroutine(SecondaryAttack());
            }
        }

        private IEnumerator SecondaryAttack()
        {
            canAttack = false;
            damage = secondaryAttackDamage;
            // trigger Animation hear
            print("attack2");
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }

        public override IEnumerator Attack()
        {
            canAttack = false;
            damage = primaryAttackDamage;
            // trigger Animation hear
            print("attack1");
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            HitImpact(collision);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            if (enemyHealth == null) { return; }
            enemyHealth.TakeDamage(damage);
        }

        private void HitImpact(Collision collision)
        {
            GameObject impact = Instantiate(hitImpact, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(impact, 0.1f);
        }
        public override bool CanAttack()
        {
            return canAttack;
        }

        public override bool IsAutomatic()
        {
            return isAutomatic;
        }
    }
}

