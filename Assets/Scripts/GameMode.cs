using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MultiplayerTennis.Core;
using UnityEngine.Networking;

namespace MultiplayerTennis
{
    public class GameMode : NetworkBehaviour
    {
        [SerializeField] int scoreToWin;
        [SerializeField] int startTimer;
        
        [Space]
        [SerializeField] BallSpawner ballSpawner;
        [SerializeField] TeamMarker[] teamWalls; 

        int topPlayerScore;
        int botPlayerScore;

        public int TopPlayerScore => topPlayerScore;
        public int BotPlayerScore => botPlayerScore;
        public int StartTimer => startTimer;

        public event UnityAction GameStarted;
        public event UnityAction StartTtmerElapsed;
        public event UnityAction ScoreChanged;
        public event UnityAction GameEnd;

        [ClientRpc]
        public void RpcGameStarted()
        {
            topPlayerScore = 0;
            botPlayerScore = 0;
            
            if(isServer)
                StartCoroutine(StartGameTimer());
            
            GameStarted?.Invoke();
        }
        
        IEnumerator StartGameTimer()
        {
            yield return new WaitForSeconds(startTimer);
            StartTtmerElapsed?.Invoke();
            SpawnBall();
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

                RpcScoreChanged(topPlayerScore, botPlayerScore);
               
        
                if (botPlayerScore < scoreToWin &&
                    topPlayerScore < scoreToWin)
                {
                    SpawnBall();
                }
                else
                {
                    RpcGameEnd();
                }
            }
        }
        
        [ClientRpc]
        void RpcScoreChanged(int topPlayerScore, int botPlayerScore)
        {
            this.topPlayerScore = topPlayerScore;
            this.botPlayerScore = botPlayerScore;
            ScoreChanged?.Invoke();
        }
        
        [ClientRpc]
        void RpcGameEnd()
        {
            GameEnd?.Invoke();
        }
    }
}