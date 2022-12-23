using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerTennis.DebugTools
{
    public class RunTimeLog : MonoBehaviour
    {
        [SerializeField] Text lineTemplate;
        [SerializeField] float timeLive;

        static RunTimeLog inst;

        public static void AddLine(string text)
        {
            if (!inst)
                inst = FindObjectOfType<RunTimeLog>();
            
            inst.AddLineInternal(text);
        }

        void AddLineInternal(string text)
        {
            Text line = Instantiate(lineTemplate, transform);
            line.text = text;
            line.gameObject.SetActive(true);
            StartCoroutine(Remove(timeLive, line.gameObject));
        }

        IEnumerator Remove(float delay, GameObject go)
        {
            yield return new WaitForSeconds(delay);
            Destroy(go);
        }
    } 
}
