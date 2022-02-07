using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;
using Cinema.PostProcessing;


// A behaviour that is attached to a playable
public class DisappearVFXPlayableBehaviour : PlayableBehaviour
{
    public GameObject dancerMeshObj;
    public GameObject warpVfxObj;
    public bool isReturnToOrigin;

    private Volume volumeProfile;
    private VisualEffect disappearVfx;

    private Material alphaDancerMaterial; 
    
    private const float RANDOM_TARGET_POSITION_MAX_X = 1.0f;
    private const float RANDOM_TARGET_POSITION_MAX_Y = 0.15f;
    private const float RANDOM_TARGET_POSITION_MAX_Z = 1.0f;
    private const float DISSOLVE_TIME = 0.4f; // progress: 0 to 1
    private const float APPEAR_TIME = 0.62f; // progress: 0 to 1

    //Start
    public override void OnGraphStart(Playable playable)
    {
        this.alphaDancerMaterial = this.dancerMeshObj.GetComponent<MeshRenderer>().sharedMaterials[0];
        this.disappearVfx = this.warpVfxObj.GetComponent<VisualEffect>();
        Debug.LogFormat("ActorTargetPosition OnGraphStart = {0}", this.disappearVfx.GetVector3("ActorTargetPosition"));

        //this.disappearVfx.SetVector3("ActorSourcePosition", Vector3.zero);
        //this.disappearVfx.SetVector3("ActorTargetPosition", Vector3.zero);
    }

    //OnDestory
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log(disappearVfx);
        Vector3 actorSourcePosition = this.disappearVfx.GetVector3("ActorTargetPosition");
        Debug.LogFormat("actorSourcePosition = {0}", actorSourcePosition);

        //
        //Set a target position randomly
        //
        Vector3 actorTargetPosition = actorSourcePosition;
        
        if (this.isReturnToOrigin)
        {
            actorTargetPosition = Vector3.zero;
        }
        else
        {
            float maxSquareDistance = 0f;
            
            for(int i = 0; i < 3; i++)
            {
                Vector3 tmpTargetPosition = new Vector3(
                    Random.Range(- RANDOM_TARGET_POSITION_MAX_X, RANDOM_TARGET_POSITION_MAX_X),
                    Random.Range(- RANDOM_TARGET_POSITION_MAX_Y, RANDOM_TARGET_POSITION_MAX_Y),
                    Random.Range(- RANDOM_TARGET_POSITION_MAX_Z, RANDOM_TARGET_POSITION_MAX_Z)
                );
                float squareDistance = (actorSourcePosition - tmpTargetPosition).sqrMagnitude;
                if (squareDistance > maxSquareDistance) {
                    actorTargetPosition = tmpTargetPosition;
                    maxSquareDistance = squareDistance;
                }
            }
        }


        //Debug.LogFormat("actorSourcePosition = {0}, actorTargetPosition = {1}", actorSourcePosition, actorTargetPosition);
        
        this.disappearVfx.SetVector3("ActorSourcePosition", actorSourcePosition);
        this.disappearVfx.SetVector3("ActorTargetPosition", actorTargetPosition);
        //this.alphaDancerMaterial.SetVector("ActorOffset", newPosition);
        this.disappearVfx.SendEvent("StartWarpVFX");
        //this.warpVfxObj.GetComponent<MeshRenderer>().enabled = true;
        //this.dancerMeshObj.GetComponent<MeshRenderer>().enabled = true;
        
        //3.21
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //this.disappearVfx.SendEvent("StopWarpVFX");
        
        //Debug.Log("OnBehaviourPause");
    }

    private IEnumerator DissolveDancer(bool isDisappeaer, float duration ) {
        
        float t0to1 = 0.0f;
        float rate = 1.0f / duration;
        while (t0to1 < 1.0f)
        {
            t0to1 += Time.deltaTime * rate;
            if (isDisappeaer) {
                alphaDancerMaterial.SetFloat("DissolvePercent", t0to1);
            } else {
                alphaDancerMaterial.SetFloat("DissolvePercent", 1.0f - t0to1);
            }
            yield return null;
        }
    }

    public float remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        //Debug.Log("progress = " + progress);
        //Debug.Log("playable.GetDuration() = " + playable.GetDuration());
        //Debug.Log("Prepare");
        if (0 <= progress && progress <= DISSOLVE_TIME)
        {
            float t0to1 = remap(progress, 0, DISSOLVE_TIME, 0, 2);
            alphaDancerMaterial.SetFloat("DissolvePercent", t0to1);
        }
        else if (APPEAR_TIME <= progress && progress <= 1.0f)
        {
            this.disappearVfx.SendEvent("StopWarpVFX");
            float t0to1 = remap(progress, APPEAR_TIME, 1.0f, 2, 0);
            alphaDancerMaterial.SetFloat("DissolvePercent", t0to1);
        }
        else if (0.5f <= progress && progress <= 0.6f)
        {
            Vector3 actorTargetPosition = this.disappearVfx.GetVector3("ActorTargetPosition");
            dancerMeshObj.transform.localPosition = actorTargetPosition;
            Debug.Log("actorTargetPosition = " + actorTargetPosition);
        }
        
    }

}
