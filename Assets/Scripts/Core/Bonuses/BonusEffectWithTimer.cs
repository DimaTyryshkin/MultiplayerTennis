using System.Collections;
using UnityEngine;

namespace MultiplayerTennis.Core.Bonuses
{
    public abstract class BonusEffectWithTimer : MonoBehaviour
    {   
        protected abstract void ResetEffect();

        public void Reset()
        {
            StopAllCoroutines();
            ResetEffect();
            DestroyImmediate(this);
        }

        protected void StartTimer(float time)
        {
            StartCoroutine(Timer(time));
        }

        IEnumerator Timer(float time)
        {
            yield return new WaitForSeconds(time);
            Reset();
        }
    }
}