using System.Collections;
using System.Collections.Generic;
using GameUltis;
using UnityEngine;

namespace Minigame.CS2D
{
    public class BulletGrenade : Bullet
    {
        protected override void Start()
        {
            Invoke(nameof(Explosion), 1f);
        }

        protected override void TriggerHandle(Collision2D collider)
        {
            if (collider.gameObject.CompareTag("Weapon") || collider.gameObject.CompareTag("Bullet"))
            {
                return;
            }

            Explosion();
        }

        public void Explosion()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
            foreach (var item in colliders)
            {
                Character c1 = item.GetComponent<Character>();
                if (c1)
                {
                    CaculateExplostion(c1);
                }
            }
            EffectManager.Instance.CreatedEffect("grenadeExplosion", transform);
            Destroy(gameObject, 0.05f);
        }

        public void CaculateExplostion(Character c)
        {
            var distance = Mathf.Max(Vector2.Distance(transform.position, c.transform.position), 1);

            if (distance < 3)
            {
                c.TakeDamage(gun.weaponSetting.damage / distance, character);
            }
        }
    }

}
