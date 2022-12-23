using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerTennis.Gui
{
    public class LobbyPresenter : MonoBehaviour
    {
        [SerializeField] GameObject matchmakingPanel;
        [SerializeField] Button hostButton;
        [SerializeField] Button clientButton;
        [SerializeField] Button abortMatchmakingButton;
        [SerializeField] Button matchmakingButton;
        [SerializeField] InputField ipField;
        [SerializeField] AutoMatchmaking matchmaking;
        
        GameNetworkManager Manager => GameNetworkManager.Inst;
        
        void Start()
        {
            Manager.Reset();
            
            matchmakingPanel.SetActive(AutoMatchmaking.isMatchmakingEnable);
            ipField.text = Manager.networkAddress;
           
            hostButton.onClick.AddListener(()=>Manager.StartHost());
            
            clientButton.onClick.AddListener(()=>
            {
                Manager.networkAddress = ipField.text;
                Manager.StartClient();
            });
            
            matchmakingButton.onClick.AddListener(() =>
            {
                AutoMatchmaking.isMatchmakingEnable = true;
                matchmakingPanel.SetActive(true);
                matchmaking.StartAutoMatchmaking();
            });
            
            abortMatchmakingButton.onClick.AddListener(() =>
            {
                matchmakingPanel.SetActive(false);
                matchmaking.Stop();
            });

            // Если последнее подключение было неудачным, то игрока выкинет обратно в меню и тут мы сразу запускаем новый поиск.
            if ( AutoMatchmaking.isMatchmakingEnable)
                matchmaking.StartAutoMatchmaking();
            else
                matchmaking.Stop();
        }
    }
}