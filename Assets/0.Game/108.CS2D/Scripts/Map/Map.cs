using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class Map : MonoBehaviour
    {
        public Transform[] pointSpawnRed;
        public Transform[] pointSpawnBlue;
        public int indexSpawnRed = 0;
        public int indexSpawnBlue = 0;

        public Transform[] randomPositonForAI;
        public Transform[] randomPositionSpawn;

        public Weapon[] allWeapons;

        public Transform SpawnPoint(int team)
        {
            Transform t = null;
            if (team == 0)
            {
                t = pointSpawnRed[indexSpawnRed];
                indexSpawnRed = (indexSpawnRed + 1) % pointSpawnRed.Length;
            }
            else if (team == 1)
            {

                t = pointSpawnBlue[indexSpawnBlue];
                indexSpawnBlue = (indexSpawnBlue + 1) % pointSpawnBlue.Length;
            }
            return t;
        }

        public Transform SpawnRandom()
        {
            Transform t = randomPositionSpawn[Random.Range(0, randomPositionSpawn.Length)];
            return t;
        }
    }
}
