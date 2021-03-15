using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    [SerializeField] AmmoSlot[] ammoSlots;
    [System.Serializable]
    private class AmmoSlot {
        public AmmoType ammoType;
        public int ammoCount;
    }

    AmmoSlot GetAmmoSlot(AmmoType ammoType) {
        foreach (AmmoSlot ammoSlot in ammoSlots) {
            if (ammoSlot.ammoType == ammoType) {
                return ammoSlot;
            }
        }
        return null;
    }

    public void ReduceAmmoCount(AmmoType ammoType) {
        GetAmmoSlot(ammoType).ammoCount--;
    }

    public int GetCurrentAmmoCount(AmmoType ammoType) {
        return GetAmmoSlot(ammoType).ammoCount;
    }

    public void IncreaseAmmoCount(AmmoType ammoType,int ammoAmount)
    {
        GetAmmoSlot(ammoType).ammoCount += ammoAmount;
    }
}
