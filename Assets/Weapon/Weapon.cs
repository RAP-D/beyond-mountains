using System;
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
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitImpact;
    bool canShoot = true;

    private void OnEnable()
    {
        canShoot = true;
    }

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
            PlayMuzzleFlash();
            ProcessRayCast();
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRayCast()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, range))
        {
            //TODO add some hit Effect
            HitImpact(hit);
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth == null) { return; }
            enemyHealth.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void HitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact,0.1f);
    }
}
