using System;
using UnityEngine;
using UnityEngine.Events;

namespace MultiplayerTennis
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] int scoreToWin;
        
        [Space]
        [SerializeField] BallSpawner ballSpawner;

        int topPlayerScore;
        int botPlayerScore;

        public int TopPlayerScore => topPlayerScore;
        public int BotPlayerScore => botPlayerScore;

        public event UnityAction GameStarted;
        public event UnityAction ScoreChanged;
        public event UnityAction GameEnd;

        public void StartGame()
        {
            topPlayerScore = 0;
            botPlayerScore = 0;
            
            SpawnBall();
            
            GameStarted?.Invoke();
        }

        void SpawnBall()
        {
            Ball newBall = ballSpawner.SpawnBall();
            newBall.Collide += OnBallCollide;
        }

        void OnBallCollide(Ball ball, GameObject go)
        {
            var playerWall = go.GetComponentInParent<PlayerWall>();
            if (playerWall)
            {
                Destroy(ball.gameObject);

                if (playerWall.isTopWall)
                    botPlayerScore++;
                else
                    topPlayerScore++;

                ScoreChanged?.Invoke();

                if (botPlayerScore < scoreToWin &&
                    topPlayerScore < scoreToWin)
                {
                    SpawnBall();
                }
                else
                {
                    GameEnd?.Invoke();
                }
            }
        }
    }
}