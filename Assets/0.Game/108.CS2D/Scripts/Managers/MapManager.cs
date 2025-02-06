using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }
        public Map[] maps;
        public Map currentMap;
        public AstarPath pathfinder;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            CreateMap(Random.Range(0, maps.Length));
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void CreateMap(int i)
        {
            Destroy(currentMap?.gameObject);
            currentMap = null;
            currentMap = Instantiate(maps[i]);
            pathfinder?.Scan();
        }
    }
}
