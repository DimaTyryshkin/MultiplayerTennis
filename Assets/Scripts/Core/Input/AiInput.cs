using UnityEngine;
using UnityEngine.Assertions;

namespace MultiplayerTennis.Core.Input
{
    public class AiInput : PointFollowInput
    { 
        protected override Vector2 Point => ball.transform.position;
        protected override bool PointAvailable => ball;

        Ball ball;

        public void SetBall(Ball ball)
        {
            Assert.IsNotNull(ball);
            this.ball = ball;
        }
    }
}