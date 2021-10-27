using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VFXProject4D {
    public class CyberCircuitGenerator:  ShaderTextureGeneratorBase
    {
        [SerializeField] private float waveScale = 7.18f;

        protected override void UpdateTexture()
        {
            this.material.SetFloat("_WaveScale", this.waveScale);
            base.UpdateTexture();
        }

    }
}