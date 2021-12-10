using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class BookPlayableBehaviour : PlayableBehaviour
{
    public GameObject cameraPivotObj;
    public BrownianMotionExtra brownianMotionExtra;

    public float rotateAngle;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //this.bookBodyObj.GetComponent<MeshRenderer>().enabled = true;
        brownianMotionExtra.pause = true;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        brownianMotionExtra.pause = false;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        cameraPivotObj.transform.Rotate(new Vector3(0, 1, 0), 360 * );

        //float vClipPercent = Mathf.Lerp(this.maxVerticalClipPercent, this.minVerticalClipPercent, progress);
        //this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", vClipPercent);
    }
}
