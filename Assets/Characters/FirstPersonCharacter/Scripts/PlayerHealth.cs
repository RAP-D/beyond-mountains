using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    void TakeDamage(float damage) {
        health -= damage;
        if (health<=0) {
            print("you are dead!!");
        }
    }
}
