using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

using Klak.Motion;

[System.Serializable]
public class BulletTimePlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> cameraPivotObj;
    public ExposedReference<GameObject> volumeObj;

    public float radiationBlurPower = 15.0f;
    public AnimationCurve radiationBlurAnimCurve;
    
    public float barrelDistortionPower = -1.0f;
    public AnimationCurve barrelDistortionAnimCurve;

    public float rotateAngle = - 720f;
    public AnimationCurve rotationSpeedAnimCurve;    

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        BulletTimePlayableBehaviour behaviour = new BulletTimePlayableBehaviour();
        behaviour.cameraPivotObj = this.cameraPivotObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.volumeProfileObj = this.volumeObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        behaviour.brownianMotionExtra = behaviour.cameraPivotObj.GetComponent<BrownianMotionExtra>();
        
        behaviour.radiationBlurPower = radiationBlurPower;
        behaviour.radiationBlurAnimCurve = radiationBlurAnimCurve;
        behaviour.barrelDistortionPower = barrelDistortionPower;
        behaviour.barrelDistortionAnimCurve = barrelDistortionAnimCurve;

        behaviour.rotationSpeedAnimCurve = rotationSpeedAnimCurve;
        behaviour.rotateAngle = rotateAngle;

        return ScriptPlayable<BulletTimePlayableBehaviour>.Create(graph, behaviour);
    }
}
