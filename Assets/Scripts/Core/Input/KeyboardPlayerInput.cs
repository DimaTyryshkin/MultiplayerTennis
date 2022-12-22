using UnityEngine;
using UnityEngine.Networking;

namespace MultiplayerTennis.Core.Input
{
    public class KeyboardPlayerInput : NetworkBehaviour
    {
        [SerializeField] TennisRacquetMovement tennisRacquet;

        void Update()
        {
            if(isServer)
            {
                float input = UnityEngine.Input.GetAxisRaw("Horizontal");
                tennisRacquet.Input(tennisRacquet.transform.position + tennisRacquet.Right * input);
            }
        }
    }
}