using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    [CreateAssetMenu(menuName = "2Player/CS2D/KeyboardSetting")]
    public class KeyboardSetting : ScriptableObject
    {
        public Keyboard player1;
        public Keyboard player2;
    }

    [System.Serializable]
    public class Keyboard
    {
        public KeyCode up, left, down, right, shoot;
    }
}
