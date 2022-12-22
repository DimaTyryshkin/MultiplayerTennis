using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MultiplayerTennis.Core
{
    public class TennisRacquetMovement : NetworkBehaviour
    {
        [SerializeField] Transform forwardProvider;
        [SerializeField] float acceleration;
        [SerializeField] float maxSpeed;
        [SerializeField] Transform viewRoot;
        [SerializeField] InfiniteWall[] leftAndRightLevelsWalls;

        
        float width;
        Vector2 velocity;
        Vector2 targetPosition;

        public float Width
        {
            get => width; 
        }

        public Vector3 Forward => forwardProvider.forward;
        public Vector3 Right => forwardProvider.right;

        void Start()
        {
            targetPosition = transform.position;
        }

        void Update()
        {
            if(isServer)
            {
                Move();
                CollideWalls();
            }
        }

        public void Input(Vector2 targetPosition)
        {
            //targetPosition.y = transform.position.y;
            this.targetPosition = targetPosition;
            
        }

        [ClientRpc]
        public void RpcSetWidth(float width)
        {
            this.width = width; 
            Vector3 localScale = viewRoot.localScale;
            localScale.x = width;
            viewRoot.localScale = localScale;
        }

        void Move()
        {
            Vector2 dirToTarget = targetPosition - (Vector2)transform.position;

            if (Vector2.Dot(dirToTarget, velocity) < 0)
                velocity = Vector2.zero;

            velocity += dirToTarget.normalized * (acceleration * Time.deltaTime);

            if (velocity.magnitude > maxSpeed)
                velocity = velocity.normalized * maxSpeed;

            Vector2 deltaPos = velocity * Time.deltaTime;
            if (deltaPos.magnitude > dirToTarget.magnitude)
            {
                transform.position = targetPosition;
                velocity = Vector2.zero;
            }
            else
            {
                transform.position += (Vector3)deltaPos;
            }
        }

        void CollideWalls()
        {
            foreach (var wall in leftAndRightLevelsWalls)
            {
                float distance = wall.GetDistanceToWall(transform.position) - width * 0.5f;
                if (distance < 0)
                {
                    velocity = Vector2.zero;
                    transform.position -= Vector3.Project(wall.Normal, forwardProvider.right).normalized * distance;
                }
            }
        }
    }
}