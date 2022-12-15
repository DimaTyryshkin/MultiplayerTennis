using UnityEngine;

namespace MultiplayerTennis
{
    public class KeyboardPlayerInput : MonoBehaviour
    {
        [SerializeField] TennisRacquetMovement tennisRacquet;

        void Update()
        {
            float input = Input.GetAxisRaw("Horizontal");
            tennisRacquet.Input(tennisRacquet.transform.position + tennisRacquet.Right * input);
        }
    }
}