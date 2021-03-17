using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Utility {
    public class BatteryPickup : MonoBehaviour
    {
        [SerializeField] int battries = 1;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Battery>().IncreaseBatteries(battries);
                Destroy(gameObject);
            }
        }
    }
}

