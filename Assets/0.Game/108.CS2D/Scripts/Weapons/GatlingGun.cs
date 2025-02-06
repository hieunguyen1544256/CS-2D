using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class GatlingGun : GunWeapon
    {
        [SerializeField] private float timeHold = 0.5f;
        private float timeCountHold = 0;
        private bool isStartHold = false;
        private int numberSlot = 0;


        protected override void Awake()
        {
            base.Awake();
            canCreateBullet = false;
        }

        private void Update()
        {
            if (owner && owner.isShoot && !isReloading)
            {
                if (!isStartHold)
                {
                    isStartHold = true;
                    SoundManager.instance.PlaySoundEffect("minigunStart", 0.5f);
                }
                timeCountHold = Mathf.Min(timeCountHold + Time.deltaTime, timeHold + 0.1f);
                InvokeRepeating(nameof(PlayAnimGatling), 0, 0.05f);
            }
            else
            {
                timeCountHold = Mathf.Max(timeCountHold - Time.deltaTime, 0);
                if (isStartHold)
                {
                    isStartHold = false;
                    SoundManager.instance.PlaySoundEffect("minigunEnd", 0.5f);
                }

                if (timeCountHold == 0)
                {
                    CancelInvoke(nameof(PlayAnimGatling));
                }
            }

            if (timeCountHold >= timeHold)
            {
                canCreateBullet = true;
            }
            else
            {
                canCreateBullet = false;
            }
        }

        public void PlayAnimGatling()
        {
            owner?.state.anim.skeleton.SetAttachment("Weapon_B", $"{Enum.GetName(typeof(NameWeapon), weaponSetting.nameWeapon)}_{numberSlot % 3}");
            numberSlot++;
        }
    }
}
