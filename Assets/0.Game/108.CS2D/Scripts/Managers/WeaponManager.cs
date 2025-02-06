using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; private set; }
        public Sprite[] muzzleFlashs;
        public Weapon[] weaponPrefabs;
        [SerializeField] private Rigidbody2D cartouche;
        internal Dictionary<NameWeapon, Weapon> weaponDic = new();

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            for (int i = 0; i < weaponPrefabs.Length; i++)
            {
                weaponDic.Add(weaponPrefabs[i].weaponSetting.nameWeapon, weaponPrefabs[i]);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public Weapon CreateWeapon(NameWeapon type)
        {
            return Instantiate(weaponDic[type]);
        }

        public void CreateCartouche(Vector3 position)
        {
            var a = Instantiate(cartouche);
            a.transform.position = position;
            a.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 5f;
            a.AddTorque(Random.Range(-300f, 300f));
            Destroy(a.gameObject, 3f);
        }
    }
}
