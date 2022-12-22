using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MultiplayerTennis.Core
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] float startSpeed;
        [SerializeField] float ballAcceleration;

        [Space] 
        [SerializeField] Transform forwardProvider;
        [SerializeField] Ball ballPrefab;
        [SerializeField] InfiniteWall[] levelWalls;
        [SerializeField] TennisRacquetCollider[] tennisRacquetColliders;


        [CanBeNull] public event UnityAction<Ball> BallSpawned;

 
        public Ball SpawnBall()
        {
            Ball newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            newBall.gameObject.SetActive(true);
            NetworkServer.Spawn(newBall.gameObject);

            Vector2 velocity = Vector2.zero;
            while (velocity == Vector2.zero)
            {
                velocity = Random.insideUnitCircle.normalized * startSpeed;
                if (Mathf.Abs(Vector2.Dot(forwardProvider.forward, velocity)) < 0.1f)
                    velocity = Vector2.zero;
            }

            newBall.Init(levelWalls, tennisRacquetColliders);
            BallSpawned?.Invoke(newBall);
            StartCoroutine(LaunchBall(newBall, velocity));
            return newBall;
        }

        IEnumerator LaunchBall(Ball ball, Vector2 speed)
        {
            ball.SetVelocity(Vector2.zero);
            ball.Acceleration = 0;
            yield return new WaitForSeconds(1);

            ball.SetVelocity(speed);
            ball.Acceleration = ballAcceleration;
        }
    }
}