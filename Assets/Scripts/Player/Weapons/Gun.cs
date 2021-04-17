using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons {
    public class Gun : Weapon
    {
        public Camera fpsCamera;
        public PlayerLook playerLook;
        [SerializeField] float range = 100f;
        RaycastHit hit;
        [SerializeField] float damage = 10;
        public Ammo ammo;
        [SerializeField] float timeBetweenShots = 0.1f;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] GameObject hitImpact;
        private bool canAttack = true;
        [SerializeField] bool isAutomatic = false;
        [SerializeField] AmmoType ammoType;
        [SerializeField] float zoomInFOV = 30f;
        [SerializeField] float zoomOutFOV = 60f;
        float zoomInMouseSens = 1f;
        float zoomOutMouseSens = 2f;
        public bool isZoomToggle = false;

        private void Start()
        {
            zoomInMouseSens = playerLook.mouseSensitivity * zoomInFOV / zoomOutFOV;
            zoomOutMouseSens = playerLook.mouseSensitivity * zoomOutFOV / zoomInFOV;
        }
        private void OnEnable()
        {
            canAttack = true;
        }

        private void OnDisable()
        {
            ZoomOut();
            isZoomToggle = false;
        }

        public  void ZoomIn()
        {
            fpsCamera.fieldOfView = zoomInFOV;
            SetMouseSensitivity(zoomInMouseSens);
        }

        private void SetMouseSensitivity(float sensitivity)
        {
            playerLook.mouseSensitivity= sensitivity;
        }

        public void ZoomOut()
        {
            fpsCamera.fieldOfView = zoomOutFOV;
            SetMouseSensitivity(zoomOutMouseSens);
        }
        public override void AlternativeAttack() {
            isZoomToggle = !isZoomToggle;
            if (isZoomToggle)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
        public override IEnumerator Attack()
        {
            canAttack = false;
            if (ammo.GetCurrentAmmoCount(ammoType) > 0)
            {
                ammo.ReduceAmmoCount(ammoType);
                PlayMuzzleFlash();
                ProcessRayCast();
            }
            yield return new WaitForSeconds(timeBetweenShots);
            canAttack = true;
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

