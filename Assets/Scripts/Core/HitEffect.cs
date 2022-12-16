using System.Collections;
using System.Linq;
using UnityEngine;

namespace MultiplayerTennis.Core
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] MeshRenderer[] wallsMeshRenderers;
        [SerializeField] Color hitColor;
        [SerializeField] GameObject[] walls;
        
        public void OnBallCollide(Ball ball, GameObject go)
        {
            if (walls.Contains(go))
                StartCoroutine(HitEffectCoroutine());
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