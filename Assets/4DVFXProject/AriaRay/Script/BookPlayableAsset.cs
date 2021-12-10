using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BookPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> bookBodyObj;
    public ExposedReference<GameObject> bookUnderNavObj;

    //Timelineでいじるパラメータ
    public bool isVerticalAnim;
    public float maxVerticalClipPercent, minVerticalClipPercent;
    public float maxHorizontalClipPercent, minHorizontalClipPercent;
    public float maxNavPercent, minNavPercent;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        BookPlayableBehaviour behaviour = new BookPlayableBehaviour();
        behaviour.bookBodyObj = this.bookBodyObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.bookUnderNavObj = this.bookUnderNavObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.isVerticalAnim = this.isVerticalAnim;
        behaviour.maxVerticalClipPercent = this.maxVerticalClipPercent;
        behaviour.minVerticalClipPercent = this.minVerticalClipPercent;
        behaviour.maxHorizontalClipPercent = this.maxHorizontalClipPercent;
        behaviour.minHorizontalClipPercent = this.minHorizontalClipPercent;
        behaviour.maxNavPercent = this.maxNavPercent;
        behaviour.minNavPercent = this.minNavPercent;

        return ScriptPlayable<BookPlayableBehaviour>.Create(graph, behaviour);
    }
}
