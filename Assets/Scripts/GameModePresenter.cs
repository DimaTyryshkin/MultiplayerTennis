using System.Collections;
using UnityEngine;
using MultiplayerTennis.Gui;
using UnityEngine.UI;

namespace MultiplayerTennis
{
    public class GameModePresenter : MonoBehaviour
    {
        [SerializeField] GameMode gameMode;

        [Space]
        [SerializeField] EndGamePanel endGamePanel;

        [Space] 
        [SerializeField] GameObject scorePanel;
        [SerializeField] Text topScoreText;
        [SerializeField] Text botScoreText;

        [Space]
        [SerializeField] GameObject timerPanel;
        [SerializeField] Text timerText;

        void Start()
        {
            ResetScorePanel();
            gameMode.GameStarted += OnGameStarted;
            gameMode.ScoreChanged += OnScoreChanged;
            gameMode.GameEnd += OnGameEnd;
        }

        void ResetScorePanel()
        {
            topScoreText.text = 0.ToString();
            botScoreText.text = 0.ToString();
        }

        IEnumerator ShowTimer()
        {
            float timer = gameMode.StartTimer;
            timerPanel.SetActive(true);
            while (timer > 0)
            {
                timerText.text = ((int)(timer + 0.9f)).ToString();
                timer -= Time.deltaTime;
                yield return null;
            }

            timerPanel.SetActive(false);
            scorePanel.SetActive(true);
        }

        void OnGameStarted()
        {
            scorePanel.SetActive(false);
            endGamePanel.gameObject.SetActive(false);
            ResetScorePanel();
            StartCoroutine(ShowTimer());
        }

        void OnScoreChanged()
        {
            topScoreText.text = gameMode.TopPlayerScore.ToString();
            botScoreText.text = gameMode.BotPlayerScore.ToString();
        }

        void OnGameEnd()
        {
            scorePanel.gameObject.SetActive(false);
            endGamePanel.gameObject.SetActive(true);

            bool isBotWinner = gameMode.BotPlayerScore > gameMode.TopPlayerScore;
            bool isLocalPlayerIsBot = gameMode.isServer;
            bool isLocalPlayerWinner = !(isBotWinner ^ isLocalPlayerIsBot);
            string msg = isLocalPlayerWinner ? "Победа" : "Проигрыш";
            endGamePanel.Draw(msg);
            endGamePanel.ClickRestart += () => GameNetworkManager.Inst.Disconnect();
        }
    }
}