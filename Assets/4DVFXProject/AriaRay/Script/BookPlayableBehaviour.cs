using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class BookPlayableBehaviour : PlayableBehaviour
{
    public GameObject bookBodyObj;
    public GameObject bookUnderNavObj;

    public bool isVerticalAnim;
    public float maxVerticalClipPercent, minVerticalClipPercent;
    public float maxHorizontalClipPercent, minHorizontalClipPercent;
    public float maxNavPercent, minNavPercent;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        this.bookBodyObj.GetComponent<MeshRenderer>().enabled = true;
        this.bookUnderNavObj.GetComponent<MeshRenderer>().enabled = true;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        if (this.isVerticalAnim) {
            float vClipPercent = Mathf.Lerp(this.maxVerticalClipPercent, this.minVerticalClipPercent, progress);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", vClipPercent);

            float nClipPercent = Mathf.Lerp(this.maxNavPercent, this.minNavPercent, progress);
            this.bookUnderNavObj.GetComponent<MeshRenderer>().material.SetFloat("_ClipPercent", nClipPercent);
        }
        else {
            float hClipPercent = Mathf.Lerp(this.maxHorizontalClipPercent, this.minHorizontalClipPercent, progress);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_HorizontalClipPercent", hClipPercent);
        }
    }
}
