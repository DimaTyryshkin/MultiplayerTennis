using System.Collections;
using UnityEngine;

namespace MultiplayerTennis
{
    public class SceneConfigurator : MonoBehaviour
    {
        [SerializeField] BallSpawner spawner;
        [SerializeField] AiInput aiInput;
        [SerializeField] HitEffect hitEffect;
        [SerializeField] GameMode gameMode;
        [SerializeField] Canvas canvas;
        
        [SerializeField] TennisRacquetMovement[] allRacquet;
        

        IEnumerator Start()
        {
            canvas.gameObject.SetActive(true);
            spawner.BallSpawned += ball => aiInput.SetBall(ball);

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
        }
    }
}