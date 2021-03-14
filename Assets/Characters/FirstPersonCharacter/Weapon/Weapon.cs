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
    [SerializeField] float timeBetweenShots=0.1f;
    bool canShoot = true;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            StartCoroutine(Shoot());
        }

    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        if (ammo.GetCurrentAmmoCount()>0) {
            ammo.ReduceAmmoCount();
            ProcessRayCast();
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void ProcessRayCast()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, range))
        {
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
