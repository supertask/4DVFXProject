using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;
using Cinema.PostProcessing;


// A behaviour that is attached to a playable
public class BulletTimePlayableBehaviour : PlayableBehaviour
{
    public GameObject cameraPivotObj;
    public GameObject volumeProfileObj;

    public BrownianMotionExtra brownianMotionExtra;

    public float radiationBlurPower;
    public AnimationCurve radiationBlurAnimCurve;
    
    public float barrelDistortionPower;
    public AnimationCurve barrelDistortionAnimCurve;
    
    public AnimationCurve rotationSpeedAnimCurve;
    public float rotateAngle;

    private RadiationBlur radiationBlur;
    private Distortion distortion;
    private Volume volumeProfile;


    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {

    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //this.bookBodyObj.GetComponent<MeshRenderer>().enabled = true;
        this.volumeProfile =  volumeProfileObj.GetComponent<Volume>();
        //Debug.Log("OnBehaviourPlay" + volumeProfile);

        if (radiationBlur == null) volumeProfile.profile.TryGet<RadiationBlur>(out radiationBlur);
        if (distortion == null) volumeProfile.profile.TryGet<Distortion>(out distortion);

        Debug.Log("radiationBlur" + radiationBlur);
        Debug.Log("distortion" + distortion);

        if (rotateAngle != 0) {
            brownianMotionExtra.pause = true;            
        }
        

        
        //StartCoroutine(RotateCamera());
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //Debug.Log("OnBehaviourPause");
        
        if (rotateAngle != 0) {
            brownianMotionExtra.pause = false;
        }
    }

    /*
    private IEnumerator RotateCamera() {
        
        var i = 0.0;
        var rate = 1.0/rotateTime;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            cameraPivotObj.transform.rotation = Quaternion.Lerp(startRot, endRot, Mathf.SmoothStep(0.0, 1.0, i));
            yield return null;
        }
    }
    */

    public float remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        Debug.Log("progress = " + progress);
        //Debug.Log("playable.GetDuration() = " + playable.GetDuration());
        
        //
        // Barrel Distortion effect
        //
        distortion.barrelDistortionPower.value = new Vector2(
            Mathf.Lerp(0, barrelDistortionPower, barrelDistortionAnimCurve.Evaluate(progress)),
            0);
            
        //
        // Radiation blur effect
        //
        radiationBlur.power.value = Mathf.Lerp(0, radiationBlurPower, radiationBlurAnimCurve.Evaluate(progress));
        
        //
        // Rotation effect
        //
        if (rotateAngle != 0) {
            float progressingDegreePerFrame = rotateAngle / ( (float)playable.GetDuration() /  Time.deltaTime);
            progressingDegreePerFrame *= rotationSpeedAnimCurve.Evaluate(progress);
            
            cameraPivotObj.transform.RotateAround(
                cameraPivotObj.transform.position, 
                Vector3.up,
                progressingDegreePerFrame);
        }
        
        /*
        Quaternion currRotation = cameraPivotObj.transform.rotation;
        
        if (progress < 0.5f) {
            cameraPivotObj.transform.rotation = Quaternion.Lerp(
                currRotation,
                Quaternion.AngleAxis(180, Vector3.up),
                remap(progress, 0, 0.5f, 0, 1)
            );
        } else {
            cameraPivotObj.transform.rotation = Quaternion.Lerp(
                currRotation,
                Quaternion.AngleAxis(360, Vector3.up),
                remap(progress, 0.5f, 1, 0, 1)
            );
        }
        */

        //        cameraPivotObj.transform.rotation  = Quaternion.Slerp(new Vector3(0, 0, 0), new Vector3(0, 360, 0));



        //float vClipPercent = Mathf.Lerp(this.maxVerticalClipPercent, this.minVerticalClipPercent, progress);
        //this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", vClipPercent);
    }
}
