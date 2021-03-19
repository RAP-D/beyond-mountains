using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Utility {
    public class Battery : MonoBehaviour
    {
        [SerializeField] int BatteryAmount = 1;
        public void IncreaseBatteries(int BatteryAmount)
        {
            this.BatteryAmount = BatteryAmount;
        }

        public bool GetBattery()
        {
            if (BatteryAmount > 0)
            {
                BatteryAmount--;
                return true;
                
            }
            else {
                return false;
            }
        }
    }
}

