using System.Collections;
using System.Collections.Generic;
using GameUltis;
using UnityEngine;

namespace Minigame.CS2D
{
    public struct CharacterProperties
    {
        public float speed;
        public float currentHealth;
        public float currentArmor;
    }

    public class Character : MonoBehaviour
    {
        public Rigidbody2D body;
        public Controller controller;

        [Header("\n***Character***\n")]
        public CharacterMoverment moverment;
        public CharacterState state;
        public CharacterGun characterGun;
        public CharacterProperties properties;
        public Transform pointGun;

        [Header("\n***UI***\n")]
        // public HealthBarUI healthBarUI;
        public ReloadUI reloadUI;
        public BulletUI bulletUI;
        public UIBase characterUI;

        [Header("********\n")]

        public Camera cameraFollow;


        internal int team;
        // status
        internal bool isMove = false;
        internal bool isShoot = false;
        internal bool isDead = false;
        internal bool isImmortal = true;
        internal bool isPlusScore = true;
        internal bool isStartGame = false;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();

        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (!GameManager.Instance.CanPlayGame) return;

            if (isShoot && characterGun.currentWeapon)
            {
                Debug.Log(11);
                characterGun.Shoot();
            }
        }

        public void Init()
        {
            isDead = false;
            isPlusScore = true;
            properties.speed = GameManager.Instance.playerSetting.speed;
            properties.currentHealth = GameManager.Instance.playerSetting.maxHealth;
            properties.currentArmor = GameManager.Instance.playerSetting.maxArmor;

            if (!isStartGame)
            {
                transform.position = MapManager.Instance.currentMap.SpawnPoint(team).position;
                isStartGame = true;
            }
            else
            {
                transform.position = MapManager.Instance.currentMap.SpawnRandom().position;
            }
            isImmortal = true;
            characterUI?.UpdateUI(TextUI.Health, properties.currentHealth);
            characterUI?.UpdateUI(TextUI.Armor, properties.currentArmor);
            EffectManager.Instance.CreatedEffect("shield", transform, new Option
            {
                timeDestroy = 3,
                isTransform = true,
            });
            //healthBarUI.UpdateHealthBar(1);

            Invoke(nameof(SetFalseImmortal), 3f);

            characterGun.Init();
            moverment.Init();
            state.InitState();
        }

        internal void CharacterDead()
        {
            SoundManager.instance.PlaySoundEffectList("die");
            Instantiate(GameManager.Instance.blood, transform.position, transform.rotation);

            EffectManager.Instance.CreatedEffect("dead", transform.position);
            characterGun.DropWeapon(TypeWeapon.MainGun);

            pointGun.GetComponent<SpriteRenderer>().sprite = null;
            isDead = true;
            moverment.StopFoodStepAudio();

            Invoke(nameof(Spawn), 3f);
            gameObject.SetActive(false);

        }

        internal void TakeDamage(float damage)
        {
            if (!gameObject.activeSelf) return;
            if (isImmortal) return;

            state.PlayAnim(2, $"Hit_{(int)characterGun.currentWeapon.weaponSetting.typeHand}", false);

            var currentArmor = properties.currentArmor;
            properties.currentArmor = Mathf.Max(properties.currentArmor - damage * 0.66f, 0);
            properties.currentHealth = Mathf.Max(properties.currentHealth - (damage - (currentArmor - properties.currentArmor)), 0);

            characterUI?.UpdateUI(TextUI.Health, properties.currentHealth);
            characterUI?.UpdateUI(TextUI.Armor, properties.currentArmor);
            //healthBarUI.UpdateHealthBar(properties.currentHealth / GameManager.Instance.playerSetting.maxHealth);

            if (properties.currentHealth == 0)
            {
                CharacterDead();
            }
        }

        internal void TakeDamage(float damage, Character characterKill)
        {
            TakeDamage(damage);
            if (properties.currentHealth == 0 && isPlusScore)
            {
                characterKill.Kill();
                isPlusScore = false;
                GameManager.Instance.IncreaseScore(team);
            }
        }

        public void FixProperties(TypeItem typeItem)
        {
            if (typeItem == TypeItem.Health)
            {
                properties.currentHealth = GameManager.Instance.playerSetting.maxHealth;
                characterUI?.UpdateUI(TextUI.Health, properties.currentHealth);
            }
            else if (typeItem == TypeItem.Armor)
            {
                properties.currentArmor = GameManager.Instance.playerSetting.maxArmor;
                characterUI?.UpdateUI(TextUI.Armor, properties.currentArmor);
            }
        }

        public void Kill()
        {
            properties.currentHealth = Mathf.Min(properties.currentHealth + 50, GameManager.Instance.playerSetting.maxHealth);

            characterUI?.UpdateUI(TextUI.Health, properties.currentHealth);
            characterUI?.UpdateUI(TextUI.Armor, properties.currentArmor);
            //healthBarUI.UpdateHealthBar(properties.currentHealth / GameManager.Instance.playerSetting.maxHealth);
        }

        public void Spawn()
        {
            gameObject.SetActive(true);

            Init();
            EffectManager.Instance.CreatedEffect("revival", transform.position);
        }

        private void SetFalseImmortal()
        {
            isImmortal = false;
        }

        #region Controll
        public void JoystickDown()
        {
            isMove = true;

            if (controller is PlayerController)
            {
                (controller as PlayerController).ActiveJoystickMove();
            }

            if (gameObject.activeSelf)
            {
                state.SetStatePlayer(StatePlayer.Move);
            }
        }

        public void JoystickUp()
        {
            isMove = false;

            if (controller is PlayerController)
            {
                (controller as PlayerController).DeActiveJoystickMove();
            }

            if (gameObject.activeSelf)
            {
                state.SetStatePlayer(StatePlayer.Idle);
            }
        }

        public void ButtonDownShoot()
        {
            // isShoot = true;
            if (GameManager.Instance.isTestMode)
            {
                isShoot = true;
            }
            else
            {
                Invoke(nameof(AcitveShoot), 0.3f);
            }

            if (controller is PlayerController)
            {
                (controller as PlayerController).ActiveJoystickShoot();
            }
        }

        public void ButtonUpShoot()
        {
            if (GameManager.Instance.isTestMode)
            {
                isShoot = false;
            }
            else
            {
                CancelInvoke(nameof(AcitveShoot));
                isShoot = false;
            }

            moverment.rotate = Vector2.zero;

            if (controller is PlayerController)
            {
                (controller as PlayerController).DeActiveJoystickShoot();
            }
        }

        public void AcitveShoot()
        {
            isShoot = true;
        }
        #endregion
    }
}
