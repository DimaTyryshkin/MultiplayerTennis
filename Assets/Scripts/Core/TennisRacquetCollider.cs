using System.Linq;
using UnityEngine;

namespace MultiplayerTennis.Core
{
    public class TennisRacquetCollider : MonoBehaviour
    {
        [SerializeField] InfiniteWall[] walls;
        
        [SerializeField] InfiniteWall frontWall;
        [SerializeField] Transform centerOfMass;

        public InfiniteWall FrontWall => frontWall;
        public Vector2 CenterOfMass => centerOfMass.position;

        public Vector2 Reflect(Vector2 dir)
        {
            return frontWall.Reflect(dir);
        }

        public bool IsBollCollide(Vector2 point, float radius)
        {
            return walls.All(w => w.GetDistanceToWall(point) < radius);
        }
    }
}