using System;
using System.Collections;
using System.Collections.Generic;
using Minigame.CS2D;
using Spine.Unity;
using UnityEngine;

namespace Minigame.FireBoyAndWaterGirl2
{
    public class ActivationObject : MonoBehaviour
    {
        [Header("Config")]
        public TypeColor typeColor;
        private float moveSpeed = 1f;

        [HideInInspector] public bool statusActive = false;
        [HideInInspector] public SkeletonAnimation anim;

        public ActivationObject[] childrenAnim;

        private void Awake()
        {
            GetComponentAnim();
            SetSkin();
        }

        protected virtual void Start()
        {
            foreach (var item in childrenAnim)
            {
                item.typeColor = typeColor;
                item.SetSkin();
            }
        }

        public virtual void Active()
        {
            statusActive = true;
            OnAnim();
            PlaySoundActive();
        }

        public virtual void DeActive()
        {
            statusActive = false;
            OffAnim();
            PlaySoundDeActive();
        }

        public void MoveObj(Vector3 position)
        {
            if (transform.position == position) return;
            transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * moveSpeed);
        }

        public void RotatateObj(float angle)
        {
            if (transform.rotation.eulerAngles.z == angle) return;
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, new(0, 0, angle), moveSpeed));
        }

        public void GetComponentAnim()
        {
            anim = GetComponent<SkeletonAnimation>();
            OffAnim();
        }

        public string GetStringTypeColor()
        {
            return Enum.GetName(typeof(TypeColor), typeColor);
        }

        public virtual void OffAnim()
        {
            // anim?.state.SetAnimation(0, $"Off_{GetStringTypeColor()}", true);
        }

        public virtual void OnAnim()
        {
            //  anim?.state.SetAnimation(0, $"On_{GetStringTypeColor()}", true);
        }

        public virtual void SetSkin()
        {
            if (anim)
            {
                anim.skeleton.SetSkin(anim.skeleton.Data.FindSkin(GetStringTypeColor()));
                anim.skeleton.SetSlotsToSetupPose();
            }
        }

        public virtual void PlaySoundActive() { }

        public virtual void PlaySoundDeActive() { }
    }
}
