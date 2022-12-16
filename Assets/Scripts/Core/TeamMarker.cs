using UnityEngine;

namespace MultiplayerTennis.Core
{
    public enum Team
    {
        NoTeam = 0,
        Top = 1,
        Bot = 2,
    }

    public class TeamMarker : MonoBehaviour
    {
        [SerializeField] Team team;

        public Team Team => team;
    }
}