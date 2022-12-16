using System.Collections;
using UnityEngine;
using MultiplayerTennis.Core;
using MultiplayerTennis.Core.Bonuses;
using MultiplayerTennis.Core.Input;

namespace MultiplayerTennis
{
    public class SceneConfigurator : MonoBehaviour
    {
        [SerializeField] BallSpawner spawner;
        [SerializeField] AiInput aiInput;
        [SerializeField] HitEffect hitEffect;
        [SerializeField] GameMode gameMode;
        [SerializeField] Canvas canvas;
        [SerializeField] BonusSystem bonusSystem;
        
        [SerializeField] TennisRacquetMovement[] allRacquet;

        [SerializeField] float racquetWidth;
        

        IEnumerator Start()
        {
            canvas.gameObject.SetActive(true);
            
            foreach (TennisRacquetMovement racquet in allRacquet)
                racquet.Width = racquetWidth;
            
            bonusSystem.SetRacquets(allRacquet);
            
            spawner.BallSpawned += ball =>
            {
                aiInput.SetBall(ball);
                bonusSystem.SetBall(ball);
            };

            spawner.BallSpawned += ball =>  
                ball.Collide += hitEffect.OnBallCollide;;

            gameMode.GameEnd += () =>
            {
                foreach (TennisRacquetMovement racquet in allRacquet)
                    racquet.gameObject.SetActive(false);
            };

            yield return null;
            yield return null;
            yield return null;

            gameMode.StartGame();
            bonusSystem.SpawnBonusWithDelay();
        }
    }
}