using System;
using System.Collections;
using UnityEngine;
using MultiplayerTennis.Core;
using MultiplayerTennis.Core.Bonuses;
using MultiplayerTennis.Core.Input;
using MultiplayerTennis.DebugTools;
using UnityEngine.Networking;

namespace MultiplayerTennis
{
    public class SceneConfigurator : NetworkBehaviour
    {
        [SerializeField] BallSpawner spawner;
        [SerializeField] AiInput aiInput;
        [SerializeField] HitEffect hitEffect;
        [SerializeField] GameMode gameMode;
        [SerializeField] Canvas gameCanvas;
        [SerializeField] BonusSystem bonusSystem;
        [SerializeField] NetworkConfigurator networkConfigurator;
        
        [SerializeField] TennisRacquetMovement serverRacquet;
        [SerializeField] TennisRacquetMovement clientRacquet;
        [SerializeField] float racquetWidth;
        
        TennisRacquetMovement[] allRacquet;
 
        void Start()
        { 
            allRacquet = new[] { serverRacquet, clientRacquet };
            
            gameCanvas.gameObject.SetActive(true);
            networkConfigurator.NetworkReady += StartGame;
            networkConfigurator.InitNetwork();
        }

        void StartGame()
        {
            LobbyPresenter.isMatchmakingEnable = false; //Удачно подцеписиль к матчу и больше не будем искать игру
            
            gameCanvas.gameObject.SetActive(true);
            
            if(isServer)
            {
                foreach (TennisRacquetMovement racquet in allRacquet)
                    racquet.RpcSetWidth(racquetWidth);

                bonusSystem.SetRacquets(allRacquet);
                
                spawner.BallSpawned += ball =>
                {
                    aiInput.SetBall(ball);
                    bonusSystem.SetBall(ball);
                };
                
                spawner.BallSpawned += ball =>  
                    ball.Collide += hitEffect.OnBallCollide;
            }
            
            gameMode.GameEnd += () =>
            {
                foreach (TennisRacquetMovement racquet in allRacquet)
                    racquet.gameObject.SetActive(false);
                
                if(isServer)
                    bonusSystem.DestroyAllBonuses();
            };

            if (isServer)
            {
                gameMode.StartTtmerElapsed += () =>
                { 
                    bonusSystem.SpawnBonusWithDelay();
                };
                gameMode.RpcGameStarted();
            }
        }
    }
}