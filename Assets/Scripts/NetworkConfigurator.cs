using MultiplayerTennis.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MultiplayerTennis
{
    public class NetworkConfigurator : NetworkBehaviour
    {
        [SerializeField] Canvas waitForNetworkCanvas;
        [SerializeField] Camera gameCamera;
        
        [SerializeField] TennisRacquetMovement serverRacquet;
        [SerializeField] TennisRacquetMovement clientRacquet;

        public event UnityAction NetworkReady;

        public void InitNetwork()
        {
            waitForNetworkCanvas.gameObject.SetActive(true);

            if (isServer)
            {
                GameNetworkManager.Inst.ServerAddPlayer += GameNetworkManager_OnServerAddPlayer;
            }
        }

        void GameNetworkManager_OnServerAddPlayer(GameNetworkPlayer player)
        {
            player.PlayerReady += () =>  OnPlayerReady(player);
        }

        void OnPlayerReady(GameNetworkPlayer player)
        {
            if (GameNetworkManager.Inst.PlayersCount == 2)
                RpcNetworkReady();
            else
                player.KikClient();
        }

        [ClientRpc]
        void RpcNetworkReady()
        {
            foreach (GameNetworkPlayer player in GameNetworkManager.Inst.GetAllPlayers())
            {
                if (player.isServer && player.isLocalPlayer)
                    player.Init(gameCamera, serverRacquet);
                else
                    player.Init(gameCamera, clientRacquet);
            } 

            waitForNetworkCanvas.gameObject.SetActive(false);
            NetworkReady?.Invoke();
        }
    }
}