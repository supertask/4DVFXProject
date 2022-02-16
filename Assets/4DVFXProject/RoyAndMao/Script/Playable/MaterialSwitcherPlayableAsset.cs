using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

namespace VFXProject4D
{

    public enum DeformationType
    {
        MidiTwist,
        Twist,
        NoiseDistortion,
        DissolveAlpha
    }

    [System.Serializable]
    public class MaterialSwitcherPlayableAsset : PlayableAsset
    {
        //public ExposedReference<GameObject> dancerMeshObj;
        public DeformationType deformationType;
        public Vector2 deformationValueRange = new Vector2(0,1);

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            //behaviourに対してパラメータを入れ込んでいく
            MaterialSwitcherPlayableBehaviour behaviour = new MaterialSwitcherPlayableBehaviour();
            //behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
            behaviour.deformationType = this.deformationType;
            behaviour.deformationValueRange = this.deformationValueRange;

            //behaviour.rotateAngle = rotateAngle;

            return ScriptPlayable<MaterialSwitcherPlayableBehaviour>.Create(graph, behaviour);
        }
    }

}