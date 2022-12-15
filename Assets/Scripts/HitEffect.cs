using System.Collections;
using UnityEngine;

namespace MultiplayerTennis
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] MeshRenderer[] wallsMeshRenderers;
        [SerializeField] Color hitColor;
        
        public void OnBallCollide(Ball ball, GameObject go)
        {
            if (go.GetComponentInParent<PlayerWall>())
            {
                StartCoroutine(HitEffectCoroutine());
            }
        }
        
        IEnumerator HitEffectCoroutine()
        {
            var oldColor = wallsMeshRenderers[0].material.color;
            foreach (var meshRenderer in wallsMeshRenderers)
                meshRenderer.material.color = hitColor;
            
            yield return new WaitForSeconds(0.15f);


            foreach (var meshRenderer in wallsMeshRenderers)
                meshRenderer.material.color = oldColor;
        } 
    }
}