using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

[System.Serializable]
public class GVoxelizerVFXPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> gvoxelizerVfxObj;
    public ExposedReference<GameObject> dancerMeshObj;
    public bool isDarkToDefaultColor;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        GVoxelizerVFXPlayableBehaviour behaviour = new GVoxelizerVFXPlayableBehaviour();
        behaviour.gvoxelizerVfxObj = this.gvoxelizerVfxObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.dancerMeshObj = this.dancerMeshObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.isDarkToDefaultColor = isDarkToDefaultColor;

        return ScriptPlayable<GVoxelizerVFXPlayableBehaviour>.Create(graph, behaviour);
    }
}
