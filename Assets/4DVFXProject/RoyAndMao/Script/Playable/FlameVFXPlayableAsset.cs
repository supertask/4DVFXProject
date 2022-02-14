using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

public enum FlameType {
    V1, V2
};

[System.Serializable]
public class FlameVFXPlayableAsset : PlayableAsset
{
    //public ExposedReference<GameObject> flameVfxV1Obj;
    //public ExposedReference<GameObject> flameVfxV2Obj;

    public ExposedReference<GameObject> dancerMeshObj;
    public bool isStart = true;
    public FlameType flameType = FlameType.V1;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        FlameVFXPlayableBehaviour behaviour = new FlameVFXPlayableBehaviour();
        //behaviour.flameVfxV1Obj = this.flameVfxV1Obj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        //behaviour.flameVfxV2Obj = this.flameVfxV2Obj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        //behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.isStart = isStart;
        behaviour.flameType = flameType;

        return ScriptPlayable<FlameVFXPlayableBehaviour>.Create(graph, behaviour);
    }
}
