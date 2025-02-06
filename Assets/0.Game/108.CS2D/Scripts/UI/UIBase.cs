using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Minigame.CS2D
{
    [Serializable]
    public class TextData
    {
        public TextMeshProUGUI health;
        public TextMeshProUGUI armor;
        public TextMeshProUGUI bullet;
    }
    public enum TextUI
    {
        Health, Armor, Bullet
    }


    public class UIBase : MonoBehaviour
    {
        public TextData[] Text;
        public Dictionary<TextUI, TextMeshProUGUI[]> Data = new();

        private void Awake()
        {
            Data = new()
            {
                { TextUI.Health, Text.Select((TextData) => TextData.health).ToArray() },
                { TextUI.Armor, Text.Select((TextData) => TextData.armor).ToArray() },
                { TextUI.Bullet, Text.Select((TextData) => TextData.bullet).ToArray() }
            };
        }

        public void UpdateUI(TextUI text, string value)
        {
            foreach (var item in Data[text])
            {
                if (item)
                {
                    item.text = value;
                }
            }
        }

        public void UpdateUI(TextUI text, float value)
        {
            UpdateUI(text, Mathf.Round(value).ToString());
        }
    }
}

