using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class WeaponSetting : ScriptableObject
    {
        public TypeHand typeHand;
        public TypeWeapon typeWeapon;
        public NameWeapon nameWeapon;
        public AudioClip[] soundEffects;
        public float damage;
        public float speedRate;
        public float weight;
        public float rangePoint;
    }
}
