namespace MultiplayerTennis.Core.Bonuses
{
    public class ChangeRacquetWidthBonusEffect : BonusEffectWithTimer
    {
        float originWidth;
        TennisRacquetMovement racquetMovement;

        public void Apply(TennisRacquetMovement racquetMovement, float factor, float time)
        {
            this.racquetMovement = racquetMovement;
            originWidth = racquetMovement.Width;
            racquetMovement.RpcSetWidth(originWidth* factor);
            StartTimer(time);
        }

        protected override void ResetEffect()
        {
            racquetMovement.RpcSetWidth(originWidth);
        } 
    }
}