using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public enum StatusWeapon
    {
        Ready,
        Waiting
    }

    public class Weapon : MonoBehaviour
    {
        public Character owner;
        public WeaponSetting weaponSetting;
        protected StatusWeapon statusWeapon = StatusWeapon.Ready;
        protected float timeDelay;
        protected Coroutine delayStatusWeapon = null;
        public SpriteRenderer image;
        public bool isPickupWeapon = true;

        protected virtual void Awake()
        {
            timeDelay = weaponSetting.speedRate;
            image = GetComponentInChildren<SpriteRenderer>();
            Init();
        }

        protected virtual void Init() { }

        public void SetEnableImage()
        {
            image.enabled = true;
            owner = null;
        }

        public void SetDisableImage()
        {
            image.enabled = false;
        }

        public virtual void Active()
        {
            if (statusWeapon == StatusWeapon.Ready)
            {
                // Fire
                Fire();

                // reset
                if (delayStatusWeapon != null) StopCoroutine(delayStatusWeapon);
                delayStatusWeapon = StartCoroutine(ResetStatusWeapon());
                statusWeapon = StatusWeapon.Waiting;
            }
        }

        protected virtual void Fire()
        {

        }

        public void PlaySoundEffect()
        {
            if (weaponSetting.soundEffects.Length != 0)
            {
                SoundManager.instance.PlaySoundEffect(weaponSetting.soundEffects[Random.Range(0, weaponSetting.soundEffects.Length)]);
            }
        }

        protected virtual IEnumerator ResetStatusWeapon()
        {
            yield return new WaitForSeconds(timeDelay);
            statusWeapon = StatusWeapon.Ready;
        }

        public void HandleCanPickUpWeapon()
        {
            StartCoroutine(PickupWeapon());
        }

        public IEnumerator PickupWeapon()
        {
            isPickupWeapon = false;
            yield return new WaitForSeconds(0.5f);
            isPickupWeapon = true;
        }
    }
}
