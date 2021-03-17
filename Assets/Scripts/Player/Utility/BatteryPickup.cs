using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Utility {
    public class BatteryPickup : MonoBehaviour
    {
        [SerializeField] int battries = 1;
        private void OnTriggerEnter(Collider other)
        {
            Battery battery;
            if (battery=other.GetComponent<Battery>())
            {
                battery.IncreaseBatteries(battries);
                Destroy(gameObject);
            }
        }
    }
}

