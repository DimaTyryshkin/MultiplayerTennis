using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MultiplayerTennis.Core
{
    public class Ball : NetworkBehaviour
    { 
        [SerializeField] float radius;
        [SerializeField, Range(0, 1)] float collideWeight;
        [SerializeField] Transform viewRoot;
 
        [Header("Debug")] 
        [SerializeField] bool isDebug;

        Vector2 velocity;
        Vector3 lastCollidePoint;
        Vector3 lastCollideDir;
        Vector3 lastCollideVelocity;
        Vector3 lastCollideResultDir;
        InfiniteWall[] walls;
        TennisRacquetCollider[] tennisRacquetColliders;

        public event UnityAction<Ball,GameObject> Collide;

        public float ActualSpeed
        {
            get => velocity.magnitude;
            set => velocity = velocity.normalized * value;
        }

        public float Acceleration { get; set; }

        public float Radius => radius;

        void Start()
        {
            viewRoot.localScale = Vector3.one * radius;
        }

        #if UNITY_EDITOR
        void OnValidate()
        {
            Undo.RecordObject(viewRoot, "Scale");
            viewRoot.localScale = Vector3.one * radius;
        }
        #endif

        void Update()
        {
            if(isServer)
            {
                Vector2 accelerationVector = velocity.normalized * Acceleration;
                velocity += accelerationVector * Time.deltaTime;
                transform.position += (Vector3)velocity * Time.deltaTime;
                CollideTennisRacquet();
                CollideWall();
            }
        }

        void OnDrawGizmos()
        {
            if (isDebug && lastCollideVelocity.sqrMagnitude > 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(lastCollidePoint, lastCollideVelocity);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(lastCollidePoint, lastCollideDir);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(lastCollidePoint, lastCollideResultDir);
            }
        }

        public void Init(InfiniteWall[] walls, TennisRacquetCollider[] tennisRacquetColliders)
        {
            Assert.IsNotNull(walls);
            Assert.IsNotNull(tennisRacquetColliders);

            this.walls = walls;
            this.tennisRacquetColliders = tennisRacquetColliders;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
         
        void CollideWall()
        {
            Vector2 pos = transform.position;

            foreach (InfiniteWall wall in walls)
            {
                float distance = wall.GetDistanceToWall(pos);
                if (distance < radius)
                {
                    velocity = wall.Reflect(velocity);
                    transform.position = wall.GetNearestPointOnSurface(transform.position, radius);
                    Collide?.Invoke(this, wall.gameObject);
                }
            }
        }

        void CollideTennisRacquet()
        {
            Vector2 pos = transform.position;

            foreach (TennisRacquetCollider collider in tennisRacquetColliders)
            {
                if (collider.IsBollCollide(pos, radius))
                {
                    velocity = collider.FrontWall.Reflect(velocity);
                    lastCollideVelocity = velocity.normalized;
                    lastCollidePoint = transform.position;

                    Vector2 collideDir = (pos - (Vector2)collider.CenterOfMass).normalized;
                    Vector2 newDir = Vector2.Lerp(velocity.normalized, collideDir, collideWeight).normalized;
                    velocity = newDir * velocity.magnitude;
                    transform.position = collider.FrontWall.GetNearestPointOnSurface(pos, radius);
                    Collide?.Invoke(this, collider.gameObject);

                    lastCollideDir = collideDir;
                    lastCollideResultDir = newDir;
                }
            }
        }
    }
}