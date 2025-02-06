using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Blood : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject, 3f);
        }
    }
}
