using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    [CreateAssetMenu(menuName = "2Player/CS2D/GunWeaponSetting")]
    public class GunWeaponSetting : WeaponSetting
    {
        public int numberOfBullutetOneMage;
        public int numberOfTotalBullets;
        public float recoil;
        public float speedBullet;
        public float criticalRate;
        public float timeToReloading;
        public int maxNumberOfTarget;
        public Bullet bulletPrefab;
        public float damageCritical;
    }
}
