using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MultiplayerTennis.Core
{
    public class Ball : MonoBehaviour
    { 
        [SerializeField] float radius;
        [SerializeField, Range(0, 1)] float collideWeight;
        [SerializeField] Transform viewRoot;

        [SerializeField] InfiniteWall[] walls;
        [SerializeField] TennisRacquetCollider[] tennisRacquetColliders;

        [Header("Debug")] [SerializeField] bool isDebug;


        float acceleration;
        Vector2 velocity;
        Vector3 lastCollidePoint;
        Vector3 lastCollideDir;
        Vector3 lastCollideVelocity;
        Vector3 lastCollideResultDir;

        public event UnityAction<Ball,GameObject> Collide;


        void Start()
        {
            viewRoot.localScale = Vector3.one * radius;
        }

        void OnValidate()
        {
            Undo.RecordObject(viewRoot, "Scale");
            viewRoot.localScale = Vector3.one * radius;
        }

        void Update()
        {
            Vector2 accelerationVector = velocity.normalized * acceleration;
            velocity += accelerationVector * Time.deltaTime;
            transform.position += (Vector3)velocity * Time.deltaTime;
            CollideTennisRacquet();
            CollideWall();
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

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        
        public void SetAcceleration(float acceleration)
        {
            this.acceleration = acceleration;
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