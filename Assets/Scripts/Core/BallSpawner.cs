using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace MultiplayerTennis.Core
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] float startSpeed;
        [SerializeField] float ballAcceleration;

        [Space] 
        [SerializeField] 
        Transform forwardProvider;
        [SerializeField] Ball ballPrefab;
        

        [CanBeNull] public event UnityAction<Ball> BallSpawned;
        

        void Start()
        {
            ballPrefab.gameObject.SetActive(false);

        }

        public Ball SpawnBall()
        {
            Ball newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            newBall.gameObject.SetActive(true);

            Vector2 velocity = Vector2.zero;
            while (velocity == Vector2.zero)
            {
                velocity = Random.insideUnitCircle.normalized * startSpeed;
                if (Vector2.Dot(forwardProvider.forward, velocity) < 0.1)
                    velocity = Vector2.zero;
            }

            BallSpawned?.Invoke(newBall);
            StartCoroutine(LaunchBall(newBall, velocity));
            return newBall;
        }
 
        IEnumerator LaunchBall(Ball ball, Vector2 speed)
        {
            ball.SetVelocity(Vector2.zero);
            ball.SetAcceleration(0);
            yield return new WaitForSeconds(1);
            
            ball.SetVelocity(speed);
            ball.SetAcceleration(ballAcceleration);
        } 
    }
}