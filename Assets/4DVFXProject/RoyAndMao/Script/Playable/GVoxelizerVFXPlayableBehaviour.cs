using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;
using Cinema.PostProcessing;


// A behaviour that is attached to a playable
public class GVoxelizerVFXPlayableBehaviour : PlayableBehaviour
{
    public GameObject dancerMeshObj;
    public GameObject gvoxelizerVfxObj;
    public bool isDarkToDefaultColor;

    private VisualEffect gvoxelizerVfx;
    private Material alphaDancerMaterial; 

    //Start
    public override void OnGraphStart(Playable playable)
    {
        this.alphaDancerMaterial = this.dancerMeshObj.GetComponent<MeshRenderer>().sharedMaterials[0];
        this.gvoxelizerVfx = this.gvoxelizerVfxObj.GetComponent<VisualEffect>();
        //Debug.LogFormat("ActorTargetPosition OnGraphStart = {0}", this.gvoxelizerVfx.GetVector3("ActorTargetPosition"));

        //this.gvoxelizerVfx.SetVector3("ActorSourcePosition", Vector3.zero);
        //this.gvoxelizerVfx.SetVector3("ActorTargetPosition", Vector3.zero);
    }

    //OnDestory
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        this.gvoxelizerVfx.SendEvent("StartGVoxelizer");
        this.alphaDancerMaterial.SetInt("HeightDissolveIsDark", this.isDarkToDefaultColor ? 1 : 0);
        this.gvoxelizerVfx.SetBool("IsDarkToDefaultColor", !this.isDarkToDefaultColor);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        this.gvoxelizerVfx.SendEvent("StopGVoxelizer");
    }


    public float remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        this.alphaDancerMaterial.SetFloat("HeightDissolveHeight", Mathf.Lerp(-0.7f, 1.8f, progress));
        this.gvoxelizerVfx.SetFloat("DeformationCurrentPoint", Mathf.Lerp(-1.5f, 3.6f, progress));

        //Debug.Log("progress = " + progress);
        
    }

}
