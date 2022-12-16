using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MultiplayerTennis.Core;

namespace MultiplayerTennis
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] int scoreToWin;
        
        [Space]
        [SerializeField] BallSpawner ballSpawner;
        [SerializeField] TeamMarker[] teamWalls;

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
            var playerWall = go.GetComponentInParent<TeamMarker>();
            if (playerWall && teamWalls.Contains(playerWall))
            {
                Destroy(ball.gameObject);

                if (playerWall.Team == Team.Top)
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