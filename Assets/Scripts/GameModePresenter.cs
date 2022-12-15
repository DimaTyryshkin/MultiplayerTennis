using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerTennis
{
    public class GameModePresenter : MonoBehaviour
    {
        [SerializeField] GameMode gameMode;
        [SerializeField] ScorePanel scorePanel;
        [SerializeField] EndGamePanel endGamePanel;

        void Start()
        {
            ResetScorePanel();
            gameMode.GameStarted += OnGameStarted;
            gameMode.ScoreChanged += OnScoreChanged;
            gameMode.GameEnd += OnGameEnd;
        }
 
        void OnGameStarted()
        {
            scorePanel.gameObject.SetActive(true);
            endGamePanel.gameObject.SetActive(false);
            ResetScorePanel();
        }
        
        void OnScoreChanged()
        {
            scorePanel.Draw(gameMode.TopPlayerScore,gameMode.BotPlayerScore);
        }
 
        void ResetScorePanel()
        {
            scorePanel.Draw(0,0);
        } 
        
        void OnGameEnd()
        {
            scorePanel.gameObject.SetActive(false);
            endGamePanel.gameObject.SetActive(true);
             
            string msg = gameMode.BotPlayerScore> gameMode.TopPlayerScore? "Победа":"Проигрыш";
            endGamePanel.Draw(msg);
            endGamePanel.ClickRestart += () => SceneManager.LoadScene(0); 
        }
    }
}