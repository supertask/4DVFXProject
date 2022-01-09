using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

[System.Serializable]
public class MaterialSwitcherPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> dancerMeshObj;


    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        MaterialSwitcherPlayableBehaviour behaviour = new MaterialSwitcherPlayableBehaviour();
        behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        //behaviour.rotateAngle = rotateAngle;

        return ScriptPlayable<MaterialSwitcherPlayableBehaviour>.Create(graph, behaviour);
    }
}
