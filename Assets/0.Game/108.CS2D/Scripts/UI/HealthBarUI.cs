using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.CS2D
{
    public class HealthBarUI : MonoBehaviour
    {
        public Image healthBarImage;

        public void UpdateHealthBar(float value)
        {
            healthBarImage.fillAmount = value;
        }
    }
}
