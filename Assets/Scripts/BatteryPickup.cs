using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField] float addIntesity = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player") {
            FlashLight flashLight = other.GetComponentInChildren<FlashLight>();
            flashLight.RestorelightAngle();
            flashLight.IncreaseLightIntensity(addIntesity);
            Destroy(gameObject);
        }
    }
}
