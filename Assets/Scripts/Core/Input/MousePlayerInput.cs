using UnityEngine;

namespace MultiplayerTennis.Core.Input
{
    public class MousePlayerInput : PointFollowInput
    {
        [SerializeField] Camera gameCamera;

        protected override Vector2 Point => gameCamera.ScreenPointToRay(UnityEngine.Input.mousePosition).origin;
    }
}