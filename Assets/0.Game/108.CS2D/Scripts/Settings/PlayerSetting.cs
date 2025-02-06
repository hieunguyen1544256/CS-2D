using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    [CreateAssetMenu(menuName = "2Player/CS2D/PlayerSetting")]
    public class PlayerSetting : ScriptableObject
    {
        public float speed;
        public float maxHealth;
        public float maxArmor;
    }

}
