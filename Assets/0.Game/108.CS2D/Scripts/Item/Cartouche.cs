using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Cartouche : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Invoke(nameof(Fade), 1.5f);
        }

        private void Fade()
        {
            sprite.DOFade(0, 0.5f);
        }

        public void DoFadeImage()
        {
            Invoke(nameof(Fade), 1.5f);
        }
    }
}
