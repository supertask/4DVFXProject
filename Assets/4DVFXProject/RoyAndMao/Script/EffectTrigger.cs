//using System.Collections;

using System;
using System.IO;

using UnityEngine;
using UnityEngine.VFX;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;

using Klak.Motion;


namespace VFXProject4D
{

    [RequireComponent(typeof(PlayableDirector))]

    public class EffectTrigger : MonoBehaviour // : SingletonMonoBehaviour<ImageEffectManager>
    {
        #region public
        
        //
        // Memo:
        // twistとnoiseDistortionは相性が良さそう
        //

        public GameObject dancerMeshObj;
        public PlayableDirector volumetricVideoDirector;
        [Space]
        
        public GameObject triangleObj;
        public GameObject rectObj;
        public GameObject scenePivotObj;

        [Space]
        public static readonly float SHAPE_ROTATION_Y_DEGREE_PER_SEC = 180.0f * 6;
        public static readonly float SHAPE_ROTATION_X_DEGREE = 180.0f;
        public static readonly float SCENE_ROTATION_Y_DEGREE = 360.0f;

        public float currentTriangleYRotateSpeed;
        public float currentTriangleXRotateSpeed;
        public float currentRectYRotateSpeed;
        public float currentRectXRotateSpeed;
        
        public bool isSaveEffectTime;

        [Space]

        [SerializeField] public VisualEffect flameV1;
        [SerializeField] public VisualEffect flameV2;
        [SerializeField] public VisualEffect warpV2;
        [SerializeField] public VisualEffect horizontalRain;
        [SerializeField] public VisualEffect swarmV3;
        [Space]

        [Header("Primitive shapes")]
        [SerializeField] private TimelineAsset[] triangleV1Timelines;
        [SerializeField] private TimelineAsset[] rectV1Timelines;
        [Space]

        [Header("Primitive particles or trails")]
        [SerializeField] private TimelineAsset[] rippleCirclesTimelines;
        [SerializeField] private TimelineAsset[] verticalRainTimelines;
        [SerializeField] private TimelineAsset[] swarmTimelines;
        [SerializeField] private TimelineAsset[] mirrorTrianglesTimelines;
        [SerializeField] private TimelineAsset[] neonLineTimelines;
        [Space]

        [Header("Human body deformations")]
        [SerializeField] private TimelineAsset[] midiTwistTimelines;
        [SerializeField] private TimelineAsset[] twistTimelines;
        [SerializeField] private TimelineAsset[] noiseDistortionTwistTimelines;
        [Space]
        
        [Header("Human body particles")]
        [SerializeField] private TimelineAsset[] warpV2Timelines;
        [SerializeField] private TimelineAsset[] gvoxelizerTimelines;
        [SerializeField] private TimelineAsset[] flameV1Timelines;
        [SerializeField] private TimelineAsset[] flameV2Timelines;
        [Space]
        
        

        private Material alphaDancerMaterial;
        private PlayableDirector director;
        private LinearMotion sceneLinearMotion;
        
        private string effectTimelineString;

        
        #endregion


        private void Awake()
        {
            this.director = this.GetComponent<PlayableDirector>();
        }
        
        private void Start()
        {
            this.alphaDancerMaterial = dancerMeshObj.GetComponent<MeshRenderer>().sharedMaterial;
            this.sceneLinearMotion = this.scenePivotObj.GetComponent<LinearMotion>();
            this.warpV2.SetVector3("ActorSourcePosition", Vector3.zero);
            this.warpV2.SetVector3("ActorTargetPosition", Vector3.zero);
            this.currentTriangleYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * 0.1f;
            this.currentTriangleXRotateSpeed = SHAPE_ROTATION_X_DEGREE * 0.0f;
            this.currentRectYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * 0.1f;
            this.currentRectXRotateSpeed = SHAPE_ROTATION_X_DEGREE * 0.0f;
        }

        private void Update()
        {
            this.KeyCheck();
            
            if (this.triangleObj.transform.localScale.x > 0 && currentTriangleYRotateSpeed > 0) {
                this.triangleObj.transform.RotateAround(this.triangleObj.transform.position, Vector3.up, this.currentTriangleYRotateSpeed * Time.deltaTime);
            }
            if (this.rectObj.transform.localScale.x > 0 && currentRectYRotateSpeed > 0) {
                this.rectObj.transform.RotateAround(this.rectObj.transform.position, Vector3.up, this.currentRectYRotateSpeed * Time.deltaTime);
            }
        }
        
        public float remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public void OnRotateScene(float midiNormalizedValue)
        {
            if (midiNormalizedValue < 0.01f) { midiNormalizedValue = 0.0f; }
            this.sceneLinearMotion.angularVelocity.y = SCENE_ROTATION_Y_DEGREE * midiNormalizedValue;
            this.SaveEffectTimePerSomeFrame("OnRotateScene", midiNormalizedValue);
            //Debug.Log(midiNormalizedValue);
        }
        //
        // Human body shader
        //
        public void OnModifyNoiseDistortion(float midiNormalizedValue)
        {
            //Debug.Log("OnNoiseDistortion value: " + midiNormalizedValue);
            alphaDancerMaterial.SetFloat("DistortionPower", midiNormalizedValue * 0.4f);
            this.SaveEffectTimeForDistortion("OnModifyNoiseDistortion", midiNormalizedValue);
        }

        public void OnModifyTwist(float midiNormalizedValue)
        {
            alphaDancerMaterial.SetFloat("TwistPercent", midiNormalizedValue);
            this.SaveEffectTimeForDistortion("OnModifyTwist", midiNormalizedValue);
        }

        public void OnStartMIDITwist()
        {
            this.director.Play(midiTwistTimelines[0]);
            this.SaveEffectTime("MIDITwistStart");
        }
        public void OnStopMIDITwist()
        {
            this.director.Play(midiTwistTimelines[1]);
            this.SaveEffectTime("MIDITwistStop");
        }

        
        
        //
        // Shapes
        //
        public void OnStartTriangleV1()
        {
            this.director.Play(triangleV1Timelines[0]);
            this.SaveEffectTime("StartTriangleV1");
        }

        public void OnStopTriangleV1()
        {
            this.director.Play(triangleV1Timelines[1]);
            this.SaveEffectTime("StopTriangleV1");
        }
        
        public void OnYRotateTriangle(float midiNormalizedValue)
        {
            this.currentTriangleYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * midiNormalizedValue;
            //Debug.Log(midiNormalizedValue);
            this.SaveEffectTimePerSomeFrame("OnYRotateTriangle", midiNormalizedValue);
        }
        
        public void OnXRotateTriangle(float midiNormalizedValue)
        {
            this.currentTriangleXRotateSpeed = 3.14f * midiNormalizedValue;
            this.triangleObj.transform.Rotate( Vector3.right, this.currentTriangleXRotateSpeed);
            this.SaveEffectTimePerSomeFrame("OnXRotateTriangle", midiNormalizedValue);

            //Debug.Log(midiNormalizedValue);
        }

        public void OnYRotateRect(float midiNormalizedValue)
        {
            this.currentRectYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * midiNormalizedValue;
            //Debug.Log(midiNormalizedValue);
            this.SaveEffectTimePerSomeFrame("OnYRotateRect", midiNormalizedValue);
        }
        
        public void OnXRotateRect(float midiNormalizedValue)
        {
            this.currentRectXRotateSpeed = 3.14f * midiNormalizedValue;
            this.rectObj.transform.Rotate( Vector3.right, this.currentRectXRotateSpeed);

            this.SaveEffectTimePerSomeFrame("OnXRotateRect", midiNormalizedValue);
            //Debug.Log(midiNormalizedValue);
        }

        public void OnStartRectV1()
        {
            this.director.Play(rectV1Timelines[0]);
            this.SaveEffectTime("StartRectV1");
        }

        public void OnStopRectV1()
        {
            this.director.Play(rectV1Timelines[1]);
            this.SaveEffectTime("StopRectV1");
        }
        
        
        //
        // Shape particles
        //
        public void OnRippleCircleUpper()
        {
            this.director.Play(rippleCirclesTimelines[0]);
            this.SaveEffectTime("RippleCircleUpper");
        }

        public void OnRippleCircleLower()
        {
            this.director.Play(rippleCirclesTimelines[1]);
            this.SaveEffectTime("RippleCircleLower");
        }
        
        public void OnVerticalRain()
        {
            this.director.Play(verticalRainTimelines[0]);
            this.SaveEffectTime("VerticalRain");
        }
        
        public void OnStartSwarm()
        {
            Debug.Log("StartSwarmV3");
            this.director.Play(swarmTimelines[0]);
            //this.swarmV3.SendEvent("StartSwarmV3");
            //this.swarmV3.SetBool("IsAlive", true);
            this.SaveEffectTime("StartSwarmV3");
        }
        public void OnStopSwarm()
        {
            Debug.Log("StopSwarmV3");
            this.director.Play(swarmTimelines[1]);
            //this.swarmV3.SendEvent("StopSwarmV3");
            //this.swarmV3.SetBool("IsAlive", false);
            this.SaveEffectTime("StopSwarmV3");
        }
        
        public void OnStartMirrorTriangles()
        {
            this.director.Play(mirrorTrianglesTimelines[0]);
            this.SaveEffectTime("StartMirrorTriangles");
        }
        public void OnStopMirrorTriangles()
        {
            this.director.Play(mirrorTrianglesTimelines[1]);
            this.SaveEffectTime("StopMirrorTriangles");
        }

        public void OnNeonLine1()
        {
            this.director.Play(neonLineTimelines[0]);
            this.SaveEffectTime("NeonLine1");
        }
        public void OnNeonLine2()
        {
            this.director.Play(neonLineTimelines[1]);
            this.SaveEffectTime("NeonLine2");
        }

        public void OnStartHorizontalRain()
        {
            this.horizontalRain.SendEvent("StartHorizontalRain");
            this.SaveEffectTime("StartHorizontalRain");
        }

        public void OnStopHorizontalRain()
        {
            this.horizontalRain.SendEvent("StopHorizontalRain");
            this.SaveEffectTime("StopHorizontalRain");
        }

        //
        // Human particle VFX
        //
        public void OnStartGVoxelizer()
        {
            this.director.Play(gvoxelizerTimelines[0]);
            this.SaveEffectTime("StartGVoxelizer");
        }
        public void OnStopGVoxelizer()
        {
            this.director.Play(gvoxelizerTimelines[1]);
            this.SaveEffectTime("StopGVoxelizer");
        }

        public void OnStartFlameV1()
        {
            this.director.Play(flameV1Timelines[0]);
            this.SaveEffectTime("StartFlameV1");
        }
        public void OnStopFlameV1()
        {
            this.director.Play(flameV1Timelines[1]);
            this.SaveEffectTime("StopFlameV1");
        }
        
        public void OnStartFlameV2()
        {
            this.director.Play(flameV2Timelines[0]);
            this.SaveEffectTime("StartFlameV2");
        }
        public void OnStopFlameV2()
        {
            this.director.Play(flameV2Timelines[1]);
            this.SaveEffectTime("StopFlameV2");
        }
        
        public void OnWarpV2()
        {
            this.director.Play(warpV2Timelines[0]);
            this.SaveEffectTime("WarpV2");
        }
        public void OnWarpV2ReturnToOrigin()
        {
            this.director.Play(warpV2Timelines[1]);
            this.SaveEffectTime("WarpV2ReturnToOrigin");
        }

        public void OnRotateYFlame(float midiNormalizedValue)
        {
            Vector3 v = new Vector3(0, remap(midiNormalizedValue, 0, 1, 1, -1) * 180f, 0);
            v.x = -12f;
            this.flameV1.SetVector3("WindAngle", v);
            v.x = -12f;
            this.flameV2.SetVector3("WindAngle", v);
            this.SaveEffectTimePerSomeFrame("OnRotateYFlame", midiNormalizedValue);
        }
        
        
        void KeyCheck()
        {
            /*
            //Horizontal Rain VFX
            if (Input.GetKeyDown(horizontalRainKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.horizontalRain.SendEvent("StopHorizontalRain");
                    this.SaveEffectTime("StopHorizontalRain");
                }
                else
                {
                    this.horizontalRain.SendEvent("StartHorizontalRain");
                    this.SaveEffectTime("StartHorizontalRain");
                }
            }

            //MIDI Twist
            if (Input.GetKeyDown(midiTwistKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.director.Play(midiTwistTimelines[1]);
                    this.SaveEffectTime("MIDITwistStop");
                }
                else
                {
                    this.director.Play(midiTwistTimelines[0]);
                    this.SaveEffectTime("MIDITwistStart");
                }
            }
            */
        }
        
        
        private void OnDestroy()
        {
            StreamWriter effectTimeWritter = new StreamWriter(Application.streamingAssetsPath + "/EffectTimeline.txt",false);
            effectTimeWritter.WriteLine(effectTimelineString);
            effectTimeWritter.Flush();
            effectTimeWritter.Close();
        }
        
        void SaveEffectTimeForDistortion(string effectName, float midiNormalizedValue)
        {
            //if (Time.frameCount % 3 == 0) { this.SaveEffectTime(effectName + ",Value=" + midiNormalizedValue); }
            if (midiNormalizedValue <= 0.025f) { this.SaveEffectTime(effectName + ",Value=" + midiNormalizedValue); }
        }
        
        void SaveEffectTimePerSomeFrame(string effectName, float midiNormalizedValue)
        {
            if (Time.frameCount % 8 == 0) { this.SaveEffectTime(effectName + ",Value=" + midiNormalizedValue); }
        }
        void SaveEffectTime(string effectName)
        {
            if (! this.isSaveEffectTime) { return; }
            //Debug.LogFormat("effectTime: {0}, effectName: {1}", this.volumetricVideoDirector.time, effectName);
            effectTimelineString += String.Format("{0},{1}\n", this.volumetricVideoDirector.time, effectName);
        }

    }
}