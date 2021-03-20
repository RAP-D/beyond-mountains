using Player.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player{
    public class WeaponController : MonoBehaviour
    {
        private Weapon weapon;
        // Update is called once per frame
        void Update()
        {
            if (weapon.isAutomatic)
            {
                if (Input.GetMouseButton(0) && weapon.canShoot)
                {
                    StartCoroutine(weapon.Shoot());
                }
            }
            else {
                if (Input.GetMouseButtonDown(0) && weapon.canShoot)
                {
                    StartCoroutine(weapon.Shoot());
                }
            }

            if (Input.GetMouseButtonDown(1)) { 
                weapon.isZoomToggle = !weapon.isZoomToggle;
                if (weapon.isZoomToggle)
                {
                    weapon.ZoomIn();
                }
                else
                {
                    weapon.ZoomOut();
                }
            }
        }
        public void SetWeapon(Weapon weapon) { this.weapon = weapon; }
    }
}
