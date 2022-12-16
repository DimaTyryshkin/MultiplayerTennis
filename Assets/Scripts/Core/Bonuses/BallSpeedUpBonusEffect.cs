namespace MultiplayerTennis.Core.Bonuses
{
    public class BallSpeedUpBonusEffect : BonusEffectWithTimer
    {
        float originSpeed;
        float originAcceleration;
        Ball ball;

        public void Apply(Ball ball, float factor, float time)
        {
            this.ball = ball;
            originSpeed = ball.ActualSpeed;
            originAcceleration = ball.Acceleration;
            ball.ActualSpeed *= factor;
            StartTimer(time);
        }

        protected override void ResetEffect()
        {
            ball.ActualSpeed = originSpeed;
            ball.Acceleration = originAcceleration;
        }
    }
}