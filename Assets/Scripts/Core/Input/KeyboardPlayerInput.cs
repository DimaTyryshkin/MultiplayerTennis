using UnityEngine;

namespace MultiplayerTennis.Core.Input
{
    public class KeyboardPlayerInput : MonoBehaviour
    {
        [SerializeField] TennisRacquetMovement tennisRacquet;

        void Update()
        {
            float input = UnityEngine.Input.GetAxisRaw("Horizontal"); 
            tennisRacquet.Input(tennisRacquet.transform.position + tennisRacquet.Right * input);
        }
    }
}