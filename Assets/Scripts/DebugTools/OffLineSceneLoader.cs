using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerTennis.DebugTools
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