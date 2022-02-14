using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

namespace VFXProject4D
{

    public enum MidiFloatType
    {
        RotateScene,
        YRotateFlame,
        
        YRotateTriangle,
        //XRotateTriangle,
        YRotateRect,
        //XRotateRect
    }

    [System.Serializable]
    public class MidiFloatPlayableAsset : PlayableAsset
    {
        //public ExposedReference<GameObject> dancerMeshObj;
        public MidiFloatType midiFloatType;
        public Vector2 midiValueRange = new Vector2(0, 0);
        //public float midiValueMiddle = 1.0f;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            //behaviourに対してパラメータを入れ込んでいく
            MidiFloatPlayableBehaviour behaviour = new MidiFloatPlayableBehaviour();
            //behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
            behaviour.midiFloatType = this.midiFloatType;
            behaviour.midiValueRange = this.midiValueRange;
            //behaviour.midiValueMiddle = this.midiValueMiddle;
            
            return ScriptPlayable<MidiFloatPlayableBehaviour>.Create(graph, behaviour);
        }
    }

}