using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] int ammoCount=10;

    public void ReduceAmmoCount() {
        ammoCount -= 1;
    }

    public int GetCurrentAmmoCount() {
        return ammoCount;
    }
}
