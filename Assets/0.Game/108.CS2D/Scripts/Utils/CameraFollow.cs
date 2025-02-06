using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public bool isFixedX = false;

        private void LateUpdate()
        {
            if (target)
            {
                transform.position = new Vector3(isFixedX ? transform.position.x : target.position.x, target.position.y, -10);
            }
        }
    }
}
