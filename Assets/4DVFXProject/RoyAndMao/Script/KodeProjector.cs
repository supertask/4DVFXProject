using UnityEngine;
using Klak.Spout;

namespace Beta
{
    public class KodeProjector : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] SpoutReceiver _receiver;
        [SerializeField] Vector2 _extent = Vector2.one;
        [SerializeField] Vector2 _offset = Vector2.one;

        [SerializeField] float _intensity = 1;

        public float intensity {
            get { return _intensity; }
            set { _intensity = value; }
        }

        #endregion

        #region MonoBehaviour functions

        void Update()
        {
            //var vparams = new Vector3(1 / _extent.x, 1 / _extent.y, _intensity);
            var vparams = new Vector4(1 / _extent.x, 1 / _extent.y, _offset.x, _offset.y);
            //Shader.SetGlobalVector("KodeLifeParams", vparams);
            Shader.SetGlobalTexture("KodeLifeMainTex",  _receiver.receivedTexture);
            Shader.SetGlobalVector("KodeLifeTilingAndOffset", vparams);
            Shader.SetGlobalFloat("KodeLifeIntensity", _intensity);
        }

        #endregion
    }
}
