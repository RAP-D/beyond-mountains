using Player.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player{
    public class WeaponController : MonoBehaviour
    {
        private Weapon Weapon;
        // Update is called once per frame
        void Update()
        {
            if (Weapon.IsAutomatic())
            {
                if (Input.GetMouseButton(0) && Weapon.CanAttack())
                {
                    StartCoroutine(Weapon.Attack());
                }
            }
            else {
                if (Input.GetMouseButtonDown(0) && Weapon.CanAttack())
                {
                    StartCoroutine(Weapon.Attack());
                }
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                Weapon.AlternativeAttack();
            }
        }
        public void SetWeapon(Weapon weapon) { this.Weapon = weapon; }
    }
}
