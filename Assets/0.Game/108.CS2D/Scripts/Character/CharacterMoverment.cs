using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class CharacterMoverment : MonoBehaviour
    {
        public Character character;
        public Vector2 move;
        public Vector2 rotate;
        public float offset;
        public Quaternion startRotation;

        public AudioSource audioSource;
        public bool isPlayingFootSound;
        public bool isFreezeRotate = false;


        private void Awake()
        {
            startRotation = transform.rotation;

        }

        private void FixedUpdate()
        {
            CharacterMove();
            CharacterRotation();
        }

        public void Init()
        {
            transform.rotation = startRotation;
        }

        private void CharacterRotation()
        {
            if (!ConditionRotate())
            {
                isFreezeRotate = false;
                return;
            }

            if (!character.isShoot)
            {
                isFreezeRotate = false;
                if (!character.isMove)
                {
                    return;
                }
            }

            // if (rotate.x == 0 && rotate.y == 0)
            // {
            //     return;
            // }

            // if (!character.isMove && !ConditionRotate())
            // {
            //     // isFreeRotate = false;
            //     return;
            // }

            // if (!GameManager.Instance.isTestMode)
            // {
            //     if (Mathf.Abs(rotate.x) < 0.5f && Mathf.Abs(rotate.y) < 0.5f && character.isShoot && !isFreezeRotate)
            //     {
            //         return;
            //     }

            //     if ((Mathf.Abs(rotate.x) > 0.5f || Mathf.Abs(rotate.y) > 0.5f) && character.isShoot)
            //     {
            //         isFreezeRotate = true;
            //     }
            // }

            if (Mathf.Abs(rotate.x) < 0.5f && Mathf.Abs(rotate.y) < 0.5f && ConditionRotate() && !isFreezeRotate)
            {
                return;
            }

            if ((Mathf.Abs(rotate.x) > 0.5f || Mathf.Abs(rotate.y) > 0.5f) && character.isShoot)
            {
                isFreezeRotate = true;
            }

            RotateFollowVector(rotate);
        }

        private void RotateFollowVector(Vector2 v)
        {
            float rotateZ = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + offset;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.z + Mathf.DeltaAngle(transform.rotation.eulerAngles.z, rotateZ), 0.5f));
        }

        private bool ConditionRotate()
        {
            return rotate != Vector2.zero;
        }

        private void CharacterMove()
        {
            if (character.isMove)
            {
                character.body.velocity = move.normalized * (character.properties.speed - character.characterGun.currentWeapon.weaponSetting.weight);
            }
            else
            {
                character.body.velocity = Vector2.zero;
            }

            if (character.body.velocity != Vector2.zero)
            {
                if (isPlayingFootSound) return;
                isPlayingFootSound = true;
                audioSource?.Play();
            }
            else
            {
                if (!isPlayingFootSound) return;
                isPlayingFootSound = false;
                audioSource?.Stop();
            }
        }

        public void StopFoodStepAudio()
        {
            isPlayingFootSound = false;
            audioSource?.Stop();
        }

        internal Vector2 GetDirectVector()
        {
            return new Vector2(Mathf.Cos((character.body.rotation - offset) * Mathf.Deg2Rad), Mathf.Sin((character.body.rotation - offset) * Mathf.Deg2Rad));
        }

        public float GetAngleTranform()
        {
            return transform.rotation.eulerAngles.z - offset;
        }
    }

}
