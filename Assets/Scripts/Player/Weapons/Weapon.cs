using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons {
    public class Weapon : MonoBehaviour
    {
        public Camera fpsCamera;
        public PlayerMovementController fpsController;
        [SerializeField] float range = 100f;
        RaycastHit hit;
        [SerializeField] float damage = 10;
        public Ammo ammo;
        [SerializeField] float timeBetweenShots = 0.1f;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] GameObject hitImpact;
        public bool canShoot = true;
        [SerializeField] AmmoType ammoType;
        [SerializeField] float zoomInFOV = 30f;
        [SerializeField] float zoomOutFOV = 60f;
        [SerializeField] float zoomInMouseSens = 1f;
        [SerializeField] float zoomOutMouseSens = 2f;
        public bool isZoomToggle = false;
        public bool isAutomatic = false;

        private void OnEnable()
        {
            canShoot = true;
        }

        private void OnDisable()
        {
            ZoomOut();
            isZoomToggle = false;
        }

        public void ZoomIn()
        {
            fpsCamera.fieldOfView = zoomInFOV;
            SetMouseSensitivity(zoomInMouseSens);
        }

        private void SetMouseSensitivity(float sensitivity)
        {
            fpsController.mouseLook.XSensitivity = sensitivity;
            fpsController.mouseLook.YSensitivity = sensitivity;
        }

        public void ZoomOut()
        {
            fpsCamera.fieldOfView = zoomOutFOV;
            SetMouseSensitivity(zoomOutMouseSens);
        }
        public IEnumerator Shoot()
        {
            canShoot = false;
            if (ammo.GetCurrentAmmoCount(ammoType) > 0)
            {
                ammo.ReduceAmmoCount(ammoType);
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
            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
            {
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
            Destroy(impact, 0.1f);
        }

        
    }
}

