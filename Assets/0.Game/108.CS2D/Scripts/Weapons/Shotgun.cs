using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Shotgun : GunWeapon
    {
        protected override void CreateBullet()
        {
            // bool isCritial = Random.Range(0, 1) < gunWeaponSetting.criticalRate;
            PlaySoundEffect();
            for (int i = 0; i < 6; i++)
            {
                CreateOneBullet(Random.Range(-20, 20));
            }
        }

        private void CreateOneBullet(float offset)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, owner.moverment.GetAngleTranform() + Random.Range(-recoil, recoil) + offset);
            Bullet b = Instantiate(gunWeaponSetting.bulletPrefab, owner.pointGun.position, rotation);
            b.character = owner;
            b.Init(this, false);
            b.Shoot();
        }
    }
}
