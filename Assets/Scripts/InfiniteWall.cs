using UnityEngine;

namespace MultiplayerTennis
{
    public class InfiniteWall : MonoBehaviour
    {
        public Vector2 Normal => transform.forward;
        
        void OnDrawGizmos()
        {
            {
                Vector3 p1 = transform.position + transform.right * 300;
                Vector3 p2 = transform.position - transform.right * 300;
                Gizmos.DrawLine(p1, p2);
            }

            {
                Vector3 p1 = transform.position;
                Vector3 p2 = transform.position + transform.forward;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(p1, p2);
            }
        }
        
        /// <summary>
        /// Если вернет расстояние больше нуля, значит точка в полуплоскости, в которую смотрит вектор transform.forward стены (нормаль стены)
        /// </summary>
        public float GetDistanceToWall(Vector2 point)
        {
            // Строим уравнение прямой, проходящей чекрез объект и находим расстояние до прямой.
            
            Vector2 normal = transform.forward;
            float d = -Vector2.Dot(transform.position, normal);
            float distanceToWall = Vector2.Dot(point, normal) + d;

            return distanceToWall;
        }

        /// <summary>
        /// Вернет ближайшую точку, лежашую на поверхности стены.
        /// </summary>
        /// <param name="offset">Смещение полученной точки от стены в направлении нормали стены</param>
        /// <returns></returns>
        public Vector2 GetNearestPointOnSurface(Vector2 point, float offset)
        {
            Vector2 normal = transform.forward;
            float distance = GetDistanceToWall(point);
            return point + normal * (offset - distance);
        }

        /// <summary>
        /// Отраджает вектор направления от стены. Как если бы вектор направления озанчал лучь света, а стена зеркало.
        /// </summary>
        public Vector2 Reflect(Vector2 direction)
        {
            return Vector2.Reflect(direction, transform.forward);
        }
    }
}