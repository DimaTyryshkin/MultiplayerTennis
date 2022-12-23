using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace MultiplayerTennis
{
    /// <summary>
    ///  Полностью автоматическое создание и подключение к игре.
    /// </summary>
    public class AutoMatchmaking : MonoBehaviour
    {
        NetworkManager Manager => GameNetworkManager.Inst;

        public static bool isMatchmakingEnable;

        IEnumerator WaitAndStartAutoMatchmaking()
        {
            yield return new WaitForSeconds(1);
            if (isMatchmakingEnable)
                StartAutoMatchmaking();
        }
 
        public void StartAutoMatchmaking()
        {
            if (!Manager.matchMaker)
                Manager.StartMatchMaker();

         
            isMatchmakingEnable = true;
            Manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        }
        
        public void Stop()
        {
            isMatchmakingEnable = true;
            Manager.StopMatchMaker();
        }

        void OnMatchList(bool success, string extendedinfo, List<MatchInfoSnapshot> responsedata)
        {
            if (!success)
            {
                StartCoroutine(WaitAndStartAutoMatchmaking());
                return;
            }

            var availableMatches = Manager.matches
                .Where(x => x.currentSize == 1)
                .ToArray();

            // Подклчаемся к случайнорй подходящей комнате
            if (availableMatches.Length > 0)
            {
                int matchIndex = Random.Range(0, availableMatches.Length);
                MatchInfoSnapshot match = Manager.matches[matchIndex];
                Manager.matchName = match.name;
                Manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
            }
            else
            {
                Manager.matchMaker.CreateMatch(Manager.matchName, Manager.matchSize, true, "", "", "", 0, 0, OnMatchCreate);
            }
        }

        void OnMatchCreate(bool success, string extendedinfo, MatchInfo responsedata)
        {
            if (!success)
            {
                StartCoroutine(WaitAndStartAutoMatchmaking());
                return;
            }

            Manager.OnMatchCreate(success, extendedinfo, responsedata);
        }

        void OnMatchJoined(bool success, string extendedinfo, MatchInfo responsedata)
        {
            if (!success)
            {
                StartCoroutine(WaitAndStartAutoMatchmaking());
                return;
            }

            Manager.OnMatchJoined(success, extendedinfo, responsedata);
        }
    }
}