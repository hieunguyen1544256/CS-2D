using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.CS2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool isTestMode = true;
        public bool CanPlayGame = false;
        internal bool isFinishGame = false;
        public Controller[] Team1;
        public Controller[] Team2;
        public List<Character> AllCharacters = new();
        public GameObject mainCanvas;
        public PlayerSetting playerSetting;
        public KeyboardSetting keyboardSetting;
        public Blood blood;

        public bool isHorizontal;

        public delegate void Callback(Character c);

        public int score1 = 0;
        public int score2 = 0;
        public bool isModeCoop = false;
        public int maxScore;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Debug.Log("Start game");
            Application.targetFrameRate = 60;
            Time.fixedDeltaTime = 0.013f;

            Physics2D.IgnoreLayerCollision(7, 7, true);
            Physics2D.IgnoreLayerCollision(7, 4, true);

            SetTeamCallback(c =>
            {
                AllCharacters.Add(c);
            });

            StartGame();

            if (isHorizontal)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
            }
        }

        private void Start()
        {
            ScoreControll.instances.SetScoreInGame(0, score1);
            ScoreControll.instances.SetScoreInGame(1, score2);

            if (isModeCoop)
            {
                maxScore = 10;
            }
            else
            {
                maxScore = 5;
            }

            StartGame();
        }

        private void OnDestroy()
        {
            Physics2D.IgnoreLayerCollision(7, 7, false);
            Physics2D.IgnoreLayerCollision(7, 4, false);
            Instance = null;
        }

        private void StartGame()
        {
            CanPlayGame = true;
            SetTeam();
            // StartCoroutine(ItemManager.Instance.CreateSupportItem());
        }

        public void SetTeamCallback(Callback c)
        {
            for (int i = 0; i < Team1.Length; i++)
            {
                c(Team1[i].character);
            }

            for (int i = 0; i < Team2.Length; i++)
            {
                c(Team2[i].character);
            }
        }

        public void SetTeam()
        {
            for (int i = 0; i < Team1.Length; i++)
            {
                Team1[i].character.team = 0;
            }

            for (int i = 0; i < Team2.Length; i++)
            {
                Team2[i].character.team = 1;
            }
        }

        public void IncreaseScore(int team)
        {
            _ = team == 0 ? score2++ : score1++;
            ScoreControll.instances.SetScoreInGame(0, score1);
            ScoreControll.instances.SetScoreInGame(1, score2);

            if (score1 >= maxScore) { FinishGame(0); }
            else if (score2 >= maxScore) { FinishGame(1); }
        }

        public void FinishGame(int team)
        {
            CanPlayGame = false;
            mainCanvas?.SetActive(false);
            ReplayCanvas.Instance.ShowReplayCanvas(team);
        }

        public void SwitchTestMode()
        {
            isTestMode = !isTestMode;
        }
    }
}
