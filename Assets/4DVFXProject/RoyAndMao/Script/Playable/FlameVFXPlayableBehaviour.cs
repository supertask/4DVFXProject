using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;
using Cinema.PostProcessing;


// A behaviour that is attached to a playable
public class FlameVFXPlayableBehaviour : PlayableBehaviour
{
    public GameObject dancerMeshObj;
    public GameObject flameVfxV1Obj;
    public GameObject flameVfxV2Obj;
    public bool isStart;
    public FlameType flameType;

    private VisualEffect flameVfxV1;
    private VisualEffect flameVfxV2;

    private Material alphaDancerMaterial;

    //Start
    public override void OnGraphStart(Playable playable)
    {
        this.alphaDancerMaterial = this.dancerMeshObj.GetComponent<MeshRenderer>().sharedMaterials[0];
        this.flameVfxV1 = this.flameVfxV1Obj.GetComponent<VisualEffect>();
        this.flameVfxV2 = this.flameVfxV2Obj.GetComponent<VisualEffect>();
    }

    //OnDestory
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log("pos: " + this.dancerMeshObj.transform.localPosition);

        if (flameType == FlameType.V1)
        {
            this.flameVfxV1.SetVector3("ActorSourcePosition", this.dancerMeshObj.transform.localPosition);
            if (isStart) {
                this.flameVfxV1.SendEvent("StartFlameV1");
            } else {
                this.flameVfxV1.SendEvent("StopFlameV1");
            }
        } else if (flameType == FlameType.V2)
        {
            this.flameVfxV2.SetVector3("ActorSourcePosition", this.dancerMeshObj.transform.localPosition);
            if (isStart) {
                this.flameVfxV2.SendEvent("StartFlameV2");
            } else {
                this.flameVfxV2.SendEvent("StopFlameV2");
            }
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        //float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        //Debug.Log("progress = " + progress);
    }

}
