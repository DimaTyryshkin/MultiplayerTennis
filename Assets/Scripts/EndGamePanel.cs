using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MultiplayerTennis
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] Text msgText;
        [SerializeField] Button restartButton;

        public event UnityAction ClickRestart;

        void Start()
        {
            restartButton.onClick.AddListener(OnClickRestart);
        }

        void OnClickRestart()
        {
            ClickRestart?.Invoke();
        }

        public void Draw(string msg)
        {
            msgText.text = msg;
        }
    }
}