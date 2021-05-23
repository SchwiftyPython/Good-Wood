using UnityEngine;

namespace Assets.Scripts
{
    public class UvScroller : MonoBehaviour 
    {
        private readonly Vector2 _uvSpeed = new Vector2( 0.0f, 0.01f );
        private Vector2 _uvOffset = Vector2.zero;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void LateUpdate() 
        {
            _uvOffset += ( _uvSpeed * Time.deltaTime );
         
            //ensure we don't scroll the texture too far 
            if(_uvOffset.x > 0.0625f) _uvOffset = new Vector2(0,_uvOffset.y);
            if(_uvOffset.y > 0.0625f) _uvOffset = new Vector2(_uvOffset.x,0);
         
            _meshRenderer.materials[0].
                SetTextureOffset(MainTex, _uvOffset);
        }
    }
}
