using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.CS2D
{
    public class ScoreControll : MonoBehaviour
    {
        public static ScoreControll instances;
        public TextMeshProUGUI[] scoreIngame;

        private void Awake()
        {
            instances = this;
        }

        private void OnDestroy()
        {
            instances = null;
        }

        public void SetScoreInGame(int team, int score)
        {
            scoreIngame[team].text = score.ToString();
        }
    }
}
