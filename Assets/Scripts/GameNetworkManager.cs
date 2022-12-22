using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MultiplayerTennis
{
    public class GameNetworkManager : NetworkManager
    {
        static GameNetworkManager inst;

        public int PlayersCount;

        public event UnityAction<GameNetworkPlayer> ServerAddPlayer;
         
        public static GameNetworkManager Inst
        {
            get
            {
                if (!inst)
                    inst = FindObjectOfType<GameNetworkManager>();

                
                return inst;
            }
        }

        public IEnumerable<GameNetworkPlayer> GetAllPlayers()
        {
            return FindObjectsOfType<GameNetworkPlayer>(); // Тут, конечно, можно поумнее получать игроков. Но пока сойдет и так.
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
            PlayersCount++;

            GameNetworkPlayer player = conn.playerControllers[0].gameObject.GetComponent<GameNetworkPlayer>();
            ServerAddPlayer?.Invoke(player);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            PlayersCount--;
        }

        public void Disconnect()
        {
            if(NetworkServer.active )
                StopHost();
            
            if(NetworkClient.active)
                StopClient();
        }
    }
}