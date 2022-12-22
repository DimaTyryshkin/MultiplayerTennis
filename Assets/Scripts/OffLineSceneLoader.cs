using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerTennis
{
    public class OffLineSceneLoader : MonoBehaviour
    {
        void Start()
        {
            if (!FindObjectOfType<GameNetworkManager>())
                SceneManager.LoadScene(0);
        }
    }
}