using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

[System.Serializable]
public class DisappearVFXPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> disappearVfxObj;
    public ExposedReference<GameObject> dissolveMeshObj;


    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        DisappearVFXPlayableBehaviour behaviour = new DisappearVFXPlayableBehaviour();
        behaviour.disappearVfxObj = this.disappearVfxObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.dissolveMeshObj = this.dissolveMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        //behaviour.rotateAngle = rotateAngle;

        return ScriptPlayable<DisappearVFXPlayableBehaviour>.Create(graph, behaviour);
    }
}
