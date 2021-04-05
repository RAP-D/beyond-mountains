using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons
{
    public abstract class Weapon : MonoBehaviour 
    {
        public abstract bool CanAttack();
        public abstract bool IsAutomatic();
        public abstract IEnumerator Attack();
        public abstract void AlternativeAttack();
    }
}

