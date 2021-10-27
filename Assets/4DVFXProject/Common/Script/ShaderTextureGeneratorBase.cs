using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComputeShaderUtil;

namespace VFXProject4D {
    public class ShaderTextureGeneratorBase : MonoBehaviour {
        public RenderTexture GetOutputTex() => outputTmpTex;

        [SerializeField] protected int resolutionWidth = 1024;
        [SerializeField] protected int resolutionHeight = 1024;

        [SerializeField] protected int lod = 0;
        [SerializeField] protected Shader targetShader;
        [SerializeField] protected RenderTexture outputTex;

        protected Material material;
        private RenderTexture outputTmpTex;
        //public CustomRenderTexture outputTmpTex;


        private void OnEnable() {
            var width = resolutionWidth >> lod;
            var height = resolutionHeight >> lod;

            this.outputTmpTex = RenderTexUtil.CreateRenderTexture(
                width, height, 0,
                RenderTextureFormat.ARGBFloat, TextureWrapMode.Clamp,
                FilterMode.Bilinear);
            this.material = new Material(this.targetShader);

            /*
            this.outputTmpTex = new CustomRenderTexture(width, height,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            this.outputTmpTex.updateMode = CustomRenderTextureUpdateMode.OnDemand;
            this.outputTmpTex.material = this.material;
            this.outputTmpTex.initializationSource = CustomRenderTextureInitializationSource.Material;
            this.outputTmpTex.initializationMaterial = this.material;
            this.outputTmpTex.doubleBuffered  = true;
            this.outputTmpTex.Initialize();
            */
        }

        private void Update() {
            this.UpdateTexture();
        }

        protected virtual void UpdateTexture() {
            //outputTmpTex.Update();

            Graphics.Blit(null, outputTmpTex, this.material);
            if (this.outputTex != null) {
                Graphics.Blit(outputTmpTex, outputTex);
            }
        }

        private void OnDisable() {
            RenderTexUtil.ReleaseRenderTexture(this.outputTmpTex);
        }
    }
}