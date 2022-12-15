using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerTennis
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] Text topScoreText;
        [SerializeField] Text botScoreText;

        public void Draw(int topScore , int botScore)
        {
            topScoreText.text = topScore.ToString();
            botScoreText.text = botScore.ToString();
        }
    }
}