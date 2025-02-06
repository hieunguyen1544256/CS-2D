using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.CS2D
{
    [System.Serializable]
    public struct GunProperties
    {
        public int bulletRemainingTotal;
        public int bulletRemainingInMage;
        public float timeToReloading;
    }

    public class GunWeapon : Weapon
    {
        public Coroutine reload;
        public bool isReloading;
        public GunProperties gunProperties;
        internal GunWeaponSetting gunWeaponSetting;
        internal float recoil = 0;
        internal bool canCreateBullet = true;
        public bool isRenewAble = true;
        public Vector3 startPosition;

        protected override void Awake()
        {
            base.Awake();
            startPosition = transform.position;
        }

        private void Start()
        {
            transform.position = startPosition;
        }

        protected override void Init()
        {
            base.Init();
            gunWeaponSetting = weaponSetting as GunWeaponSetting;
            gunProperties.bulletRemainingTotal = gunWeaponSetting.numberOfTotalBullets;
            gunProperties.bulletRemainingInMage = gunWeaponSetting.numberOfBullutetOneMage;
            gunProperties.timeToReloading = gunWeaponSetting.timeToReloading;
            canCreateBullet = true;
            isReloading = false;
        }

        protected override void Fire()
        {
            base.Fire();

            if (gunProperties.bulletRemainingInMage > 0 && !isReloading)
            {
                if (canCreateBullet)
                {
                    gunProperties.bulletRemainingInMage--;
                    gunProperties.bulletRemainingTotal--;
                    PlayAnimGun();
                    Invoke(nameof(FireBullet), 0.1f);
                    // CreateBullet(); 

                    recoil = Mathf.Min(recoil + gunWeaponSetting.recoil / 5, gunWeaponSetting.recoil);
                }
            }
            else
            {
                AcitveReload();
            }
        }

        private void PlayAnimGun()
        {
            owner.state.PlayAnim(1, $"Shoot_{(int)weaponSetting.typeHand}", false);
        }

        private void FixedUpdate()
        {
            if (recoil > 0)
            {
                recoil = Mathf.Max(recoil - 10f * Time.fixedDeltaTime, 0);
            }
        }

        public virtual void FireBullet()
        {
            if (owner)
            {
                CreateBullet();
                TurnOnMuzzuleFlash();
                PlaySoundEffect();
                if (owner?.bulletUI)
                {
                    SetUI();
                }

                if (gunProperties.bulletRemainingInMage == 0)
                {
                    AcitveReload();
                }
            }
        }

        protected virtual void CreateBullet()
        {
            Quaternion rotation = Quaternion.Euler(0, 0, owner.moverment.GetAngleTranform() + Random.Range(-recoil, recoil));
            Bullet b = Instantiate(gunWeaponSetting.bulletPrefab, owner.pointGun.position, rotation);
            b.character = owner;

            float a = Random.Range(0f, 1f);
            bool isCritial;
            if (a < gunWeaponSetting.criticalRate)
            {
                isCritial = true;
            }
            else
            {
                isCritial = false;
            }
            b.Init(this, isCritial);
            b.Shoot();
        }

        public IEnumerator Reloading()
        {
            SoundManager.instance.PlaySoundEffect("reload");
            owner.reloadUI?.TurnOnReloading(gunWeaponSetting.timeToReloading);
            yield return new WaitForSeconds(gunProperties.timeToReloading);
            isReloading = false;
            var offset = gunProperties.bulletRemainingTotal - gunWeaponSetting.numberOfBullutetOneMage;

            // Debug.Log(offset);

            if (offset <= 0)
            {
                gunProperties.bulletRemainingInMage = gunProperties.bulletRemainingTotal;
                // gunProperties.bulletRemainingTotal = 0;
            }
            else
            {
                gunProperties.bulletRemainingInMage = gunWeaponSetting.numberOfBullutetOneMage;
                // gunProperties.bulletRemainingTotal -= offset;
            }

            if (owner?.bulletUI)
            {
                SetUI();
            }
        }

        public void AcitveReload()
        {
            if (isReloading) return;

            isReloading = true;

            if ((gunProperties.bulletRemainingTotal - gunProperties.bulletRemainingInMage == 0) || (gunWeaponSetting.numberOfBullutetOneMage == gunProperties.bulletRemainingInMage))
            {
                if (gunProperties.bulletRemainingTotal == 0)
                {
                    Debug.Log("Ran out of bullet");
                    // destroy gun
                    DestroyGun();
                    return;
                }
                else
                {
                    Debug.Log("Not reload");
                    isReloading = false;
                    return;
                }

            }

            reload = StartCoroutine(Reloading());
        }

        public void TurnOnMuzzuleFlash()
        {
            WeaponManager.Instance.CreateCartouche(owner.pointGun.position);
            owner.pointGun.GetComponent<SpriteRenderer>().sprite = WeaponManager.Instance.muzzleFlashs[Random.Range(0, WeaponManager.Instance.muzzleFlashs.Length)];
            CancelInvoke(nameof(TurnOffMuzzuleFlash));
            Invoke(nameof(TurnOffMuzzuleFlash), 0.1f);
        }

        protected void TurnOffMuzzuleFlash()
        {
            if (owner)
            {
                owner.pointGun.GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        public void DestroyGun()
        {
            TurnOffMuzzuleFlash();
            owner.pointGun.GetComponent<SpriteRenderer>().sprite = null;
            // owner.characterGun.
            owner.characterGun.ThrowWeapon();
            if (isRenewAble)
            {
                // RenewGun();
                // Invoke(nameof(RenewGun), 0.4f);
                RenewGun();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RenewGun()
        {
            DOTween.Clear(transform);
            transform.position = startPosition;
            Init();
        }

        public void StopReload()
        {
            isReloading = false;
            if (reload != null)
            {
                StopCoroutine(reload);
            }
            reload = null;
        }

        public void SetUI()
        {
            owner?.characterUI.UpdateUI(TextUI.Bullet, gunProperties.bulletRemainingInMage + "/" + (gunProperties.bulletRemainingTotal - gunProperties.bulletRemainingInMage));
            //bulletUI.SetText(gunProperties.bulletRemainingInMage, gunProperties.bulletRemainingTotal - gunProperties.bulletRemainingInMage);
        }
    }
}

