using UnityEngine;

namespace MultiplayerTennis
{
    public class MousePlayerInput : PointFollowInput
    {
        [SerializeField] Camera gameCamera;

        protected override Vector2 Point => gameCamera.ScreenPointToRay(Input.mousePosition).origin;
    }
}