using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.CS2D
{
    public class CharacterGun : MonoBehaviour
    {
        public Dictionary<TypeWeapon, Weapon> allWeapons = new();
        public Character character;
        public NameWeapon DefaultWeapon;
        public Weapon currentWeapon;
        public GameObject laser;
        public GameObject crosshair;

        public float range = 0.5f;

        private void Awake()
        {
            allWeapons.Add(TypeWeapon.MainGun, null);
            allWeapons.Add(TypeWeapon.Melee, null);

        }

        public void Init()
        {
            DestroyWeapon(currentWeapon);
            DestroyWeapon(allWeapons[TypeWeapon.Melee]);
            DestroyWeapon(allWeapons[TypeWeapon.MainGun]);

            CreateWeapon(DefaultWeapon);
        }

        public void Shoot()
        {
            currentWeapon?.Active();
        }

        public void HandlePickUpWeapon(Weapon weapon)
        {
            DropWeapon(weapon.weaponSetting.typeWeapon);

            currentWeapon = weapon;
            SetupWeapon(weapon);
        }

        public void DropWeapon(TypeWeapon typeWeapon, bool isThrow = false)
        {
            if (!allWeapons[typeWeapon]) return;

            var weapon = allWeapons[typeWeapon];

            if (isThrow)
            {
                var angle = character.moverment.GetAngleTranform() * Mathf.Deg2Rad;
                var vector2 = weapon.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f;
                var direct = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2.5f;

                weapon.HandleCanPickUpWeapon();

                var cols = Physics2D.OverlapCircleAll(vector2, 0.7f);
                bool isStand = false;
                foreach (var item in cols)
                {
                    if (item.CompareTag("Wall"))
                    {
                        isStand = true;
                        break;
                    }
                }

                if (!isStand)
                {
                    HandleThrowWeapon(weapon, weapon.transform.position + direct);
                }

                // if ((bool)!(Physics2D.OverlapCircle(vector2, 0.7f)?.CompareTag("Wall")))
                // {
                //     HandleThrowWeapon(weapon, weapon.transform.position + direct);
                //     // weapon.transform.position = weapon.transform.position + direct;
                // }
            }

            weapon.transform.SetParent(null);
            weapon.SetEnableImage();
            allWeapons[typeWeapon] = null;
            currentWeapon = null;
        }

        public void HandleThrowWeapon(Weapon weapon, Vector3 position)
        {
            float value = 0.3f;
            weapon.transform.DOMove(position, value);
            weapon.transform.DOScale(weapon.transform.localScale * 1.5f, value / 2).OnComplete(() =>
            {
                weapon.transform.DOScale(weapon.transform.localScale / 1.5f, value / 2);
            });
        }

        public void ThrowWeapon(TypeWeapon typeWeapon = TypeWeapon.MainGun)
        {
            if (allWeapons[typeWeapon])
            {
                DropWeapon(typeWeapon, true);
                SwitchMeleeWeapon();
            }
        }

        public void DestroyWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            Destroy(weapon.gameObject);
        }

        public void CreateWeapon(NameWeapon nameWeapon)
        {
            currentWeapon = WeaponManager.Instance.CreateWeapon(nameWeapon);
            SetupWeapon(currentWeapon);
        }

        public void SetupWeapon(Weapon weapon)
        {
            allWeapons[weapon.weaponSetting.typeWeapon] = weapon;

            // if AWP -> zoom

            weapon.owner = character;
            weapon.transform.SetParent(transform);
            weapon.transform.position = transform.position;
            weapon.SetDisableImage();
            SetupSlotWeapon();
        }

        public void SetupSlotWeapon()
        {
            character.state.anim.skeleton.SetAttachment("Weapon_A", "None");
            character.state.anim.skeleton.SetAttachment("Weapon_B", "None");

            character.pointGun.transform.localPosition = new(0, -currentWeapon.weaponSetting.rangePoint, 0);

            if (currentWeapon.weaponSetting.typeWeapon == TypeWeapon.Melee)
            {
                TweenCam(9);
                if (laser)
                {
                    laser.SetActive(false);
                }
                if (crosshair)
                {
                    crosshair.SetActive(false);
                }
                character.bulletUI?.gameObject.SetActive(false);
            }
            else
            {
                if (currentWeapon.weaponSetting.nameWeapon == NameWeapon.AWP && character.cameraFollow)
                {
                    TweenCam(12);
                    if (laser)
                    {
                        laser.SetActive(true);
                    }
                    if (crosshair)
                    {
                        crosshair.SetActive(false);
                    }
                }
                else
                {
                    TweenCam(9);
                    if (laser)
                    {
                        laser.SetActive(false);
                    }
                    if (crosshair)
                    {
                        crosshair.SetActive(true);
                    }
                }

                if (character.bulletUI)
                {
                    character.bulletUI.gameObject.SetActive(true);
                    (currentWeapon as GunWeapon).SetUI();
                }
            }

            if (currentWeapon.weaponSetting.typeHand == TypeHand.TypeOne)
            {
                character.state.anim.skeleton.SetAttachment("Weapon_A", Enum.GetName(typeof(NameWeapon), currentWeapon.weaponSetting.nameWeapon));
            }
            else
            {
                character.state.anim.skeleton.SetAttachment("Weapon_B", Enum.GetName(typeof(NameWeapon), currentWeapon.weaponSetting.nameWeapon));
            }

            character.state.UpdateAnimation();
        }

        public void TweenCam(float value)
        {
            character.cameraFollow.DOOrthoSize(value, 0.5f);
        }

        // change currrent to melee
        public void SwitchMeleeWeapon()
        {
            // allWeapons[TypeWeapon.MainGun] = null;
            currentWeapon = allWeapons[TypeWeapon.Melee];
            SetupSlotWeapon();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Weapon"))
            {
                var weapon = other.gameObject.GetComponent<Weapon>();
                if (!allWeapons[weapon.weaponSetting.typeWeapon] && !weapon.owner && weapon.isPickupWeapon)
                {
                    currentWeapon = weapon;
                    SetupWeapon(weapon);
                }
            }
        }

        public void PickupWeapon()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

            character.reloadUI?.TurnOffReloading();

            (allWeapons[TypeWeapon.MainGun] as GunWeapon)?.StopReload();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Weapon"))
                {
                    var weapon = colliders[i].GetComponent<Weapon>();
                    if (!weapon.owner && weapon.isPickupWeapon)
                    {
                        if (allWeapons[TypeWeapon.MainGun])
                        {
                            ThrowWeapon();
                        }

                        HandlePickUpWeapon(colliders[i].GetComponent<Weapon>());
                        return;
                    }
                }
            }

            if (allWeapons[TypeWeapon.MainGun])
            {
                ThrowWeapon();
            }
        }

        public void ReloadGun()
        {
            if (currentWeapon.weaponSetting.typeWeapon == TypeWeapon.MainGun)
            {
                (currentWeapon as GunWeapon).AcitveReload();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
