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
    public class MidiFloatPlayableBehaviour : PlayableBehaviour
    {
        //public GameObject dancerMeshObj;
        public MidiFloatType midiFloatType;
        public Vector2 midiValueRange;
        //public float midiValueMiddle;

        private EffectTrigger effectTrigger;
        private LinearMotion sceneLinearMotion;

        //private Material alphaDancerMaterial; 
        
        //Start
        public override void OnGraphStart(Playable playable)
        {
            this.effectTrigger = GameObject.FindObjectOfType<EffectTrigger>();
            this.sceneLinearMotion = this.effectTrigger.scenePivotObj.GetComponent<LinearMotion>();
        }

        //OnDestory
        public override void OnGraphStop(Playable playable)
        {
        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            /*
            MeshRenderer renderer = this.dancerMeshObj.GetComponent<MeshRenderer>();
            if (this.alphaDancerMaterial == null && renderer != null) {
                this.alphaDancerMaterial = renderer.sharedMaterials[0];
            }
            */
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
            
            /*
            float midiValue;
            if (progress < 0.5) {
                midiValue = Mathf.Lerp(midiValueRange.x, midiValueMiddle, progress);
            } else {
                midiValue = Mathf.Lerp(midiValueMiddle, midiValueRange.y, progress);
            }
            */
            float midiValue = Mathf.Lerp(midiValueRange.x, midiValueRange.y, progress);


            
            this.effectTrigger.isSaveEffectTime = false;
            // Change parameters
            if (this.midiFloatType == MidiFloatType.RotateScene)
            {
                this.effectTrigger.OnRotateScene(midiValue);
            } 
            else if (this.midiFloatType == MidiFloatType.YRotateFlame)
            {
                this.effectTrigger.OnRotateYFlame(midiValue);
            }
            else if (this.midiFloatType == MidiFloatType.YRotateTriangle)
            {
                this.effectTrigger.OnYRotateTriangle(midiValue);
            }
            else if (this.midiFloatType == MidiFloatType.YRotateRect)
            {
                this.effectTrigger.OnYRotateRect(midiValue);
            }
            this.effectTrigger.isSaveEffectTime = true;
            /*
    public enum MidiFloatType
    {
        RotateScene,
        YRotateFlame,
        
        YRotateTriangle,
        XRotateTriangle,
        YRotateRect,
        XRotateRect
    }
            */
            
        }

    }

}