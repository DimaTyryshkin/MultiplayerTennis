using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace MultiplayerTennis.Core.Bonuses
{
    public class BonusSystem : NetworkBehaviour
    {
        [SerializeField] float bonusRadius;
        [SerializeField] float spawnDelay;
        [SerializeField] Transform minSpawnPosition;
        [SerializeField] Transform maxSpawnPosition;
        [SerializeField] BonusOnBoard[] bonusPrefabs;

        [Space] 
        [SerializeField] float widthFactor;
        [SerializeField] float speedFactor;
        [SerializeField] float effectTime;

        Ball ball;
        Team lastCollideTeam;
        List<BonusOnBoard> bonusesOnBoard;
        Dictionary<Team,TennisRacquetMovement> teamToRacquet;

        [ServerCallback]
        void Update()
        {
            if(!ball)
                return;
           
            if(lastCollideTeam == Team.NoTeam)
                return;
            
            if(bonusesOnBoard == null)
                return;

            for (int i = 0; i < bonusesOnBoard.Count; i++)
            {
                BonusOnBoard bonus = bonusesOnBoard[i];
                float distance = Vector2.Distance(bonus.transform.position, ball.transform.position);
                if (distance < ball.Radius + bonusRadius)
                {
                    ApplyBonus(bonus, lastCollideTeam);
                    bonusesOnBoard.RemoveAt(i);
                    i--;
                    
                    SpawnBonusWithDelay();
                }
            }
        }
        
        public void SpawnBonusWithDelay()
        {
            StartCoroutine(SpawnTimer(spawnDelay));
        }
        
        public void SetBall(Ball ball)
        {
            Assert.IsNotNull(ball);
            this.ball = ball;
            lastCollideTeam = Team.NoTeam;
            ball.Collide += OnBallCollide;
        }
        
        public void SetRacquets(TennisRacquetMovement[] racquetMovement)
        {
            Assert.IsNotNull(racquetMovement);
            teamToRacquet = new Dictionary<Team, TennisRacquetMovement>();

            foreach (TennisRacquetMovement racquet in racquetMovement)
            {
                TeamMarker teamMarker = racquet.GetComponent<TeamMarker>();
                teamToRacquet[teamMarker.Team] = racquet;
            }
        }

        void OnBallCollide(Ball ball, GameObject obj)
        {
            if (obj.GetComponent<TennisRacquetMovement>())
            {
                var teamMarker = obj.GetComponent<TeamMarker>();
                if (teamMarker)
                    lastCollideTeam = teamMarker.Team;
            }
        }

        public void DestroyAllBonuses()
        {
            foreach (var bonus in bonusesOnBoard)
                Destroy(bonus.gameObject);
            
            bonusesOnBoard.Clear();
        }

        IEnumerator SpawnTimer(float delay)
        {
            yield return new WaitForSeconds(delay);
            SpawnBonusRandom();
        }
        
        void SpawnBonusRandom()
        {
            int index = Random.Range(0, bonusPrefabs.Length);
            SpawnBonus(bonusPrefabs[index]);
        }
        
        void SpawnBonus(BonusOnBoard bonusPrefab)
        {
            if (bonusesOnBoard == null)
                bonusesOnBoard = new List<BonusOnBoard>();

            Vector3 position = Vector2.Lerp(minSpawnPosition.position, maxSpawnPosition.position, Random.Range(0f, 1f));

            BonusOnBoard newBonus = Instantiate(bonusPrefab, position, Quaternion.identity);
            newBonus.gameObject.SetActive(true);
            NetworkServer.Spawn(newBonus.gameObject);
            bonusesOnBoard.Add(newBonus);
        }

        void ApplyBonus(BonusOnBoard bonus, Team team)
        {
            Team otherTeam = team == Team.Bot ? Team.Top : Team.Bot; 

            if (bonus.type == BonusType.IncreaseSelfWidth)
            {
                TennisRacquetMovement racquetMovement = teamToRacquet[team];
                ChangeRacquetWidthBonusEffect bonusEffect = AddBonus<ChangeRacquetWidthBonusEffect>(racquetMovement.gameObject);
                bonusEffect.Apply(racquetMovement, widthFactor, effectTime);
            }
            
            if (bonus.type == BonusType.DecreaseEnemyWidth)
            {
                TennisRacquetMovement otherRacquetMovement = teamToRacquet[otherTeam];
                ChangeRacquetWidthBonusEffect bonusEffect = AddBonus<ChangeRacquetWidthBonusEffect>(otherRacquetMovement.gameObject);
                bonusEffect.Apply(otherRacquetMovement, 1f / widthFactor, effectTime);
            }
            
            if (bonus.type == BonusType.BallSpeedUp)
            {
                BallSpeedUpBonusEffect bonusEffect = AddBonus<BallSpeedUpBonusEffect>(ball.gameObject);
                bonusEffect.Apply(ball, speedFactor, effectTime);
            }

            Destroy(bonus.gameObject);
        }

        T AddBonus<T>(GameObject target) where T : BonusEffectWithTimer
        {
            BonusEffectWithTimer oldEffect = target.GetComponent<BonusEffectWithTimer>();
            if (oldEffect)
                oldEffect.Reset();

            return target.AddComponent<T>();
        }
    }
}