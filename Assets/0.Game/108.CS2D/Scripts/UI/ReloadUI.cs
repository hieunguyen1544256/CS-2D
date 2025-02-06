using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.CS2D
{
    public class ReloadUI : MonoBehaviour
    {
        public float timeCount = 0;
        public TextMeshProUGUI textTime;
        public float stepTime = 0.1f;
        public Coroutine startReload;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void TurnOnReloading(float value)
        {
            timeCount = value;
            gameObject.SetActive(true);
            startReload = StartCoroutine(CountTimeReload());
        }

        public void TurnOffReloading()
        {
            startReload = null;
            gameObject.SetActive(false);
        }

        public IEnumerator CountTimeReload()
        {
            while (true)
            {
                if (timeCount <= 0)
                {
                    TurnOffReloading();
                    break;
                }
                else
                {
                    textTime.text = Common.RoundFloat(timeCount);
                    // (Mathf.Round(timeCount * 10f) / 10f).ToString();
                    yield return new WaitForSeconds(stepTime);
                    timeCount -= 0.1f;
                }
            }
        }
    }
}
