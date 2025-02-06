using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Minigame.CS2D
{
    public enum StatePlayer
    {
        Move,
        Idle,
    }

    public class CharacterState : MonoBehaviour
    {
        public Character character;
        public SkeletonAnimation anim;
        public bool canUpdate = true;
        private StatePlayer currentState = StatePlayer.Idle;

        private void Start()
        {
            // InitState();
        }

        public void InitState()
        {
            currentState = StatePlayer.Idle;
            UpdateAnimation();
        }

        public void UpdateAnimation()
        {
            if (GameManager.Instance.isFinishGame) return;

            if (!canUpdate) return;

            if (currentState == StatePlayer.Move)
            {
                anim?.state.SetAnimation(0, $"Walk_{(int)character.characterGun.currentWeapon.weaponSetting.typeHand}", true);
            }
            else if (currentState == StatePlayer.Idle)
            {
                anim?.state.SetAnimation(0, $"Idle_{(int)character.characterGun.currentWeapon.weaponSetting.typeHand}", true);
            }
        }

        public void SetStatePlayer(StatePlayer name)
        {
            if (currentState == name)
            {
                return;
            }
            else
            {
                currentState = name;
                UpdateAnimation();
            }
        }

        public void PlayAnim(int track, string name, bool value)
        {
            anim.state.SetAnimation(track, name, value);
        }
    }
}
