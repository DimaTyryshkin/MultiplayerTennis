using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MultiplayerTennis.Gui
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] Text msgText;
        [SerializeField] Button restartButton;
        [SerializeField] float showButtonDelay = 3;

        public event UnityAction ClickRestart;

        void Start()
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(OnClickRestart);
            StartCoroutine(ShowButtonWithDelay());
        }
        
        IEnumerator ShowButtonWithDelay()
        {
            yield return new WaitForSeconds(showButtonDelay);
            restartButton.gameObject.SetActive(true);
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