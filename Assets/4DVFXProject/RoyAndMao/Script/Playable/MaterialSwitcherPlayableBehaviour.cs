using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;
using Cinema.PostProcessing;

namespace VFXProject4D
{

    // A behaviour that is attached to a playable
    public class MaterialSwitcherPlayableBehaviour : PlayableBehaviour
    {
        public GameObject dancerMeshObj;
        public DeformationType deformationType;
        public Vector2 deformationValueRange;

        private Material alphaDancerMaterial; 
        
        //Start
        public override void OnGraphStart(Playable playable)
        {
        }

        //OnDestory
        public override void OnGraphStop(Playable playable)
        {
        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            MeshRenderer renderer = this.dancerMeshObj.GetComponent<MeshRenderer>();
            if (this.alphaDancerMaterial == null && renderer != null) {
            this.alphaDancerMaterial = renderer.sharedMaterials[0];

            }
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
        }

        public float remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0

            float deformationValue = Mathf.Lerp(deformationValueRange.x, deformationValueRange.y, progress);
            if (this.deformationType == DeformationType.MidiTwist)
            {
                alphaDancerMaterial.SetFloat("MidiTwistPercent", deformationValue);
            } 
            else if (this.deformationType == DeformationType.Twist)
            {
                alphaDancerMaterial.SetFloat("TwistPercent", deformationValue);
            }
            else if (this.deformationType == DeformationType.NoiseDistortion)
            {
                alphaDancerMaterial.SetFloat("DistortionPower", deformationValue);
            }
        }

    }

}