using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Controller : MonoBehaviour
    {
        public Character character;

        protected virtual void Start()
        {
            character.controller = this;
        }

        protected virtual void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            if (!GameManager.Instance.CanPlayGame || GameManager.Instance.isFinishGame || !character)
            {
                return;
            }
        }
    }
}
