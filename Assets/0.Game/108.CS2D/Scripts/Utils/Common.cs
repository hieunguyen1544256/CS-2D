using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public enum TypeItem
    {
        Health,
        Armor,
    }
    public enum TypeHand
    {
        TypeTwo,
        TypeOne,
    }
    public enum TypeWeapon
    {
        Melee,
        MainGun,
    }

    public enum TypeColor
    {
        Blue,
        Green,
        Purple,
        Red,
        Yellow,
    }

    public enum NameWeapon
    {
        AK_47,
        AWP,
        Gatling,
        Uzi,
        Knife,
        Shotgun,
        DE,
        Pistol,
        Grenade_launcher
    }

    public static class Common
    {
        public static string RoundFloat(float value)
        {
            return (Mathf.Round(value * 10f) / 10f).ToString();
        }

        public static Color Get(TypeColor typeColor)
        {
            Color c = Color.cyan;

            switch (typeColor)
            {
                case TypeColor.Blue:
                    {
                        c = new Color(0.172549f, 0.6588235f, 0.6117647f);
                        break;
                    }
                case TypeColor.Green:
                    {
                        c = Color.green;
                        break;
                    }

                case TypeColor.Purple:
                    {
                        c = new Color(0.9254902f, 0.1254902f, 0.9170322f);
                        break;
                    }
                case TypeColor.Red:
                    {
                        c = Color.red;
                        break;
                    }
                case TypeColor.Yellow:
                    {
                        c = Color.yellow;
                        break;
                    }
                default:
                    break;

            }
            return c;
        }

    }
}
