using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]Camera fpCamera;
    [SerializeField] float range = 100f;
    RaycastHit hit;
    [SerializeField] float damage = 10;
    [SerializeField] Ammo ammo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        if (ammo.GetCurrentAmmoCount()>0) {
            ammo.ReduceAmmoCount();
            ProcessRayCast();
        }
    }

    private void ProcessRayCast()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, range))
        {
            print("fdsfdsf");
            //TODO add some hit Effect
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth == null) { return; }
            enemyHealth.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }
}
