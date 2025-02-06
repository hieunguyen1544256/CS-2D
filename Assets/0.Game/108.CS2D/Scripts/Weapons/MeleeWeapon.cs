using System.Collections;
using System.Collections.Generic;
using GameUltis;
using UnityEngine;

namespace Minigame.CS2D
{
    public class MeleeWeapon : Weapon
    {
        private void Start()
        {

        }

        protected override void Fire()
        {
            base.Fire();
            PlaySoundEffect();
            owner.state.PlayAnim(1, "Knife_0", false);
            owner.state.anim.state.AddEmptyAnimation(1, 0.1f, 0);

            // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, (weaponSetting as MeleeWeaponSetting).range);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(owner.pointGun.position, (weaponSetting as MeleeWeaponSetting).range);

            foreach (var item in colliders)
            {
                if (item.name == "Collider")
                {
                    EffectManager.Instance.CreatedEffect("slash", owner.pointGun.position);
                }

                Character c = item.GetComponent<Character>();

                if (!c || c.team == owner.team)
                {
                    continue;
                }

                SoundManager.instance.PlaySoundEffect("meleeHit");
                EffectManager.Instance.CreatedEffect("beHitMelee", c.transform.position);
                c.TakeDamage(weaponSetting.damage, owner);

                // if (c.properties.currentHealth == 0)
                // {
                //     owner.Kill();
                // }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(owner.pointGun.position, (weaponSetting as MeleeWeaponSetting).range);
        }
    }
}
