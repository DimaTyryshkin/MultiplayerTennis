using UnityEngine;

namespace MultiplayerTennis
{
    public abstract class PointFollowInput : MonoBehaviour
    {
        [SerializeField] TennisRacquetMovement tennisRacquet;

        protected abstract Vector2 Point { get; }
        protected virtual bool PointAvailable => true;

        void Update()
        {
            Vector2 pos = tennisRacquet.transform.position;
                
            if (PointAvailable)
            {
                Vector2 dirToBall = Point - pos;
                Vector2 input = Vector3.Project(dirToBall, tennisRacquet.Right);
                tennisRacquet.Input(pos + input);
            }
            else
            {
                tennisRacquet.Input(pos);
            }
        }
    }
}