using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameUltis;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Bullet : MonoBehaviour
    {
        public GunWeapon gun;
        protected Rigidbody2D rb;
        private int numberOfTarget;
        public bool isCritial = false;
        public Character character;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Destroy(gameObject, 2f);
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }

        public virtual void Init(GunWeapon gunWeapon, bool isCritial)
        {
            gun = gunWeapon;
            this.isCritial = isCritial;

            numberOfTarget = gun.gunWeaponSetting.maxNumberOfTarget;

            foreach (var item in character.team == 0 ? GameManager.Instance.Team1 : GameManager.Instance.Team2)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>());
            }
        }

        public virtual void Shoot(float offset = 0)
        {
            float angle = (transform.rotation.eulerAngles.z + offset) * Mathf.Deg2Rad;
            Vector2 vectorF = new(Mathf.Cos(angle), Mathf.Sin(angle));
            GetComponent<Rigidbody2D>().velocity = vectorF * gun.gunWeaponSetting.speedBullet;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            TriggerHandle(other);
        }

        protected virtual void TriggerHandle(Collision2D collider)
        {
            // if (collider.gameObject.CompareTag("Weapon") || collider.gameObject.CompareTag("Bullet"))
            // {
            //     return;
            // }

            if (collider.gameObject.name.Contains("Player"))
            {
                var player = collider.gameObject.GetComponent<Character>();
                if (player.team == character.team)
                {
                    return;
                }

                player.TakeDamage(isCritial ? gun.weaponSetting.damage * (gun.weaponSetting as GunWeaponSetting).damageCritical : gun.weaponSetting.damage, character);

                // if (player.properties.currentHealth == 0)
                // {
                //     character.Kill();
                // }

                EffectManager.Instance.CreatedEffect("beHit", transform);
                SoundManager.instance.PlaySoundEffectList("bulletHit");
                numberOfTarget--;
            }
            else if (collider.gameObject.name.Contains("Collider"))
            {
                Vector3 hit = collider.contacts[0].point;
                EffectManager.Instance.CreatedEffect("hitInWall", hit);
            }
            else if (collider.gameObject.name.Contains("ColliderWood"))
            {
                Vector3 hit = collider.contacts[0].point;
                EffectManager.Instance.CreatedEffect("hitInWood", hit);
            }
            else
            {
                Vector3 hit = collider.contacts[0].point;
                EffectManager.Instance.CreatedEffect("hitInWall", hit);
            }

            Destroy(gameObject);
        }
    }
}
