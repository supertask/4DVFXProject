using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

[System.Serializable]
public class DisappearVFXPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> warpVfxObj;
    public ExposedReference<GameObject> dancerMeshObj;
    public bool isReturnToOrigin;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        DisappearVFXPlayableBehaviour behaviour = new DisappearVFXPlayableBehaviour();
        behaviour.warpVfxObj = this.warpVfxObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.isReturnToOrigin = this.isReturnToOrigin;

        return ScriptPlayable<DisappearVFXPlayableBehaviour>.Create(graph, behaviour);
    }
}
