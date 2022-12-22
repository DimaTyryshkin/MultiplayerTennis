using MultiplayerTennis.DebugTools;
using UnityEngine;
using UnityEngine.Networking;

public delegate void EventLogDelegate(int a); 

namespace MultiplayerTennis
{
    public class Test : NetworkBehaviour
    {
        [SyncEvent] public event EventLogDelegate EventLog;

        float timeNextEvent;
        int n;

        void Start()
        {
            if (!isServer)
                EventLog += Log;
        }

        void Log(int arg0)
        {
            //RunTimeLog.AddLine(arg0.ToString());
        }

        void Update()
        {
            if (isServer)
            {
                if (Time.time > timeNextEvent)
                {
                    timeNextEvent = Time.time + 2;
                    EventLog?.Invoke(n);
                    n++;
                    //Debug.Log(n.ToString());
                }
            }
        }

        [ContextMenu("SendRpc")]
        public void SendRpc()
        {
            RpcDoOnClient();
        }

        [ClientRpc]
        public void RpcDoOnClient()
        {
            //RunTimeLog.AddLine("SendRpc");
        }

        [ContextMenu("Command")]
        public void Command()
        {
            CmdCommand();
        }

        [Command]
        public void CmdCommand()
        {
            //RunTimeLog.AddLine("Command");
        }
    }
}