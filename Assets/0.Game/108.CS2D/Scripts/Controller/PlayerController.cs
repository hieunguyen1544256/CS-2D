using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.CS2D
{
    public class PlayerController : Controller
    {
        public FixedJoystick joystickMove;
        public FixedJoystick joystickShoot;
        public Keyboard keyboard;

        protected override void Start()
        {
            base.Start();
            if (name == "Player1")
            {
                keyboard = GameManager.Instance.keyboardSetting.player1;
            }
            else if (name == "Player2")
            {
                keyboard = GameManager.Instance.keyboardSetting.player2;
            }
        }

        protected override void Move()
        {
            base.Move();

            var v = CalculateDirection();
            character.moverment.move = v;
            if (v != Vector2.zero)
            {
                character.JoystickDown();
            }
            else
            {
                character.JoystickUp();
            }

            if (joystickShoot.Direction != Vector2.zero)
            {
                character.moverment.rotate = joystickShoot.Direction;
            }
            else
            {
                character.moverment.rotate = v;
            }

            if (Input.GetKeyDown(keyboard.shoot))
            {
                character.ButtonDownShoot();
            }

            if (Input.GetKeyUp(keyboard.shoot))
            {
                character.ButtonUpShoot();
            }
        }

        private Vector2 CalculateDirection()
        {
            Vector2 move = new();
            move += joystickMove.Direction;

            if (Input.GetKey(keyboard.left))
            {
                move += Vector2.left;
            }
            if (Input.GetKey(keyboard.right))
            {
                move += Vector2.right;
            }
            if (Input.GetKey(keyboard.up))
            {
                move += Vector2.up;
            }
            if (Input.GetKey(keyboard.down))
            {
                move += Vector2.down;
            }

            return move.normalized;
        }

        protected override void Update()
        {
            base.Update();
            // if (Input.GetKey(KeyCode.Mouse0))
            // {
            //     Vector3 mousePos = Input.mousePosition;
            //     Debug.Log(Camera.main.gameObject.name);

            //     Debug.Log(Camera.main.ScreenToWorldPoint(mousePos));
            // }
        }

        public void ActiveJoystickMove()
        {
            ActiveJoystick(joystickMove);
        }

        public void DeActiveJoystickMove()
        {
            DeActiveJoystick(joystickMove);
        }

        public void ActiveJoystickShoot()
        {
            ActiveJoystick(joystickShoot);
        }

        public void DeActiveJoystickShoot()
        {
            DeActiveJoystick(joystickShoot);
        }

        public void ActiveJoystick(Joystick joystick)
        {
            var images = joystick.GetComponentsInChildren<Image>();

            foreach (var item in images)
            {
                item.color = new Color(1, 1, 1, 0.2f);
            }
        }

        public void DeActiveJoystick(Joystick joystick)
        {
            var images = joystick.GetComponentsInChildren<Image>();

            foreach (var item in images)
            {
                item.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
