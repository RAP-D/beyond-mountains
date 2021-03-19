using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Utility {
    public class FlashLight : MonoBehaviour
    {
        [SerializeField] float lightDecay = 0.1f;
        [SerializeField] float angleDecay = 1f;
        [SerializeField] float minimumAngle = 40f;
        float angle;
        float intensity;
        Light flashLight;
        [SerializeField] Battery battery;
        // Start is called before the first frame update
        void Start()
        {
            flashLight = GetComponent<Light>();
            angle = flashLight.spotAngle;
            this.intensity = flashLight.intensity;

        }

        // Update is called once per frame
        void Update()
        {
            DecreaseFlashLight();
            if (flashLight.intensity == 0 && battery.GetBattery()) {
                Restorelight();
            }
        }

        private void DecreaseFlashLight()
        {
            flashLight.intensity -= lightDecay * Time.deltaTime;
            if (flashLight.spotAngle >= minimumAngle)
            {
                flashLight.spotAngle -= angleDecay * Time.deltaTime;
            }
        }

        public void Restorelight()
        {
            flashLight.spotAngle = angle;
            flashLight.intensity = this.intensity;
        }

    }

}

