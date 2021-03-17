using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player{
    public class WeaponSwitcher : MonoBehaviour
    {
        // Start is called before the first frame update
        int currentWeaponIndex = 0;
        void Start()
        {
            SetActiveWeapon();
        }

        private void SetActiveWeapon()
        {
            int weaponIndex = 0;
            foreach (Transform weapon in transform)
            {
                if (currentWeaponIndex == weaponIndex)
                {
                    weapon.gameObject.SetActive(true);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                weaponIndex++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            int previousWeaponIndex = currentWeaponIndex;
            ProcessMouseWheelInput();
            ProcessKeyboardInput();
            if (previousWeaponIndex != currentWeaponIndex) { SetActiveWeapon(); }
        }

        private void ProcessKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeaponIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeaponIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentWeaponIndex = 2;
            }
        }

        private void ProcessMouseWheelInput()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (transform.childCount - 1 <= currentWeaponIndex) { currentWeaponIndex = 0; }
                else { currentWeaponIndex++; }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentWeaponIndex <= 0)
                {
                    currentWeaponIndex = transform.childCount - 1;
                }
                else
                {
                    currentWeaponIndex--;
                }
            }
        }
    }

}
