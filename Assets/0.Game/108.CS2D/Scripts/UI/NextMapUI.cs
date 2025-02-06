using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class NextMapUI : MonoBehaviour
    {
        public int mapIndex = 0;

        public void NextMap()
        {
            GameManager.Instance.SetTeamCallback((Character c) =>
            {
                c.isStartGame = false;
            });
            MapManager.Instance.CreateMap(mapIndex % MapManager.Instance.maps.Length);

            GameManager.Instance.SetTeamCallback((Character c) =>
            {
                c.Init();
            });
            mapIndex++;
        }
    }
}
