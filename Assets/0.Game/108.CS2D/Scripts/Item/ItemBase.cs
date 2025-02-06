using System.Collections;
using System.Collections.Generic;
using GameUltis;
using UnityEngine;

namespace Minigame.CS2D
{
    public class ItemBase : MonoBehaviour
    {
        public TypeItem typeItem;
        protected bool isTrigger = false;


        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<Character>() && !isTrigger)
            {
                isTrigger = true;

                var player = collider.GetComponent<Character>();
                ItemCollected(player);
                gameObject.SetActive(false);
                Invoke(nameof(ResetItem), 10f);
            }
        }

        private void ItemCollected(Character player)
        {
            if (typeItem == TypeItem.Health)
            {
                EffectManager.Instance.CreatedEffect("healing", player.transform);
                SoundManager.instance.PlaySoundEffect("heal");
            }
            else
            {
                EffectManager.Instance.CreatedEffect("armor", player.transform);
                SoundManager.instance.PlaySoundEffect("armor");
            }
            player.FixProperties(typeItem);
        }

        private void ResetItem()
        {
            isTrigger = false;
            gameObject.SetActive(true);
        }
    }
}
