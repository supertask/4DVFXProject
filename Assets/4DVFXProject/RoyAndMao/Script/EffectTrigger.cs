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

        // Particle VFX
        [SerializeField] private KeyCode horizontalRainKey = KeyCode.Q;
        [Space]
        
        //Human Particle VFX
        [SerializeField] private KeyCode flameV1Key = KeyCode.A;
        [SerializeField] private KeyCode flameV2Key = KeyCode.S;
        [SerializeField] private KeyCode warpV2Key = KeyCode.D;

        [Space]

        //Human Vertex shader VFX
        [SerializeField] private KeyCode midiTwistKey = KeyCode.Z;
        [Space]
        
        public GameObject dancerMeshObj;
        public PlayableDirector volumetricVideoDirector;
        [Space]
        
        public GameObject triangleObj;
        public GameObject rectObj;
        [Space]

        [SerializeField] public VisualEffect flameV1;
        [SerializeField] public VisualEffect flameV2;
        [SerializeField] public VisualEffect warpV2;
        [SerializeField] public VisualEffect horizontalRain;

        [SerializeField] private TimelineAsset[] triangleV1Timelines;
        [SerializeField] private TimelineAsset[] rectV1Timelines;
        [SerializeField] private TimelineAsset[] rippleCirclesTimelines;

        [Space]

        [SerializeField] private TimelineAsset[] midiTwistTimelines;
        [SerializeField] private TimelineAsset[] twistTimelines;
        [SerializeField] private TimelineAsset[] noiseDistortionTwistTimelines;
        [Space]
        
        [SerializeField] private TimelineAsset[] warpV2Timelines;
        [SerializeField] private TimelineAsset[] gvoxelizerTimelines;
        [SerializeField] private TimelineAsset[] flameV1Timelines;
        [SerializeField] private TimelineAsset[] flameV2Timelines;


        [Space]
        
        

        private Material alphaDancerMaterial;
        private PlayableDirector director;
        private string effectTimelineString;
        
        private float SHAPE_ROTATION_Y_DEGREE_PER_SEC = 180.0f * 6;
        private float SHAPE_ROTATION_X_DEGREE = 180.0f;
        private float currentTriangleYRotateSpeed;
        private float currentTriangleXRotateSpeed;
        private float currentRectYRotateSpeed;
        private float currentRectXRotateSpeed;
        
        #endregion


        private void Start()
        {
            this.alphaDancerMaterial = dancerMeshObj.GetComponent<MeshRenderer>().sharedMaterial;
            this.director = this.GetComponent<PlayableDirector>();
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


        //
        // Human body shader
        //
        public void OnModifyNoiseDistortion(float midiNomalizedValue)
        {
            //Debug.Log("OnNoiseDistortion value: " + midiNomalizedValue);
            alphaDancerMaterial.SetFloat("DistortionPower", midiNomalizedValue * 0.4f);
            //this.SaveEffectTime("NoiseDistortion,Value=" + midiNomalizedValue);
        }
        
        public void OnModifyTwist(float midiNomalizedValue)
        {
            alphaDancerMaterial.SetFloat("TwistPercent", midiNomalizedValue);
        }
        
        public void OnRotateYFlame(float midiNomalizedValue)
        {
            Vector3 v = new Vector3(0, remap(midiNomalizedValue, 0, 1, 1, -1) * 180f, 0);
            v.x = -12f;
            this.flameV1.SetVector3("WindAngle", v);
            v.x = -12f;
            this.flameV2.SetVector3("WindAngle", v);
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
        
        public void OnYRotateTriangle(float midiNomalizedValue)
        {
            this.currentTriangleYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * midiNomalizedValue;
            //Debug.Log(midiNomalizedValue);
        }
        
        public void OnXRotateTriangle(float midiNomalizedValue)
        {
            this.currentTriangleXRotateSpeed = 3.14f * midiNomalizedValue;
            this.triangleObj.transform.Rotate( Vector3.right, this.currentTriangleXRotateSpeed);

            //Debug.Log(midiNomalizedValue);
        }

        public void OnYRotateRect(float midiNomalizedValue)
        {
            this.currentRectYRotateSpeed = SHAPE_ROTATION_Y_DEGREE_PER_SEC * midiNomalizedValue;
            //Debug.Log(midiNomalizedValue);
        }
        
        public void OnXRotateRect(float midiNomalizedValue)
        {
            this.currentRectXRotateSpeed = 3.14f * midiNomalizedValue;
            this.rectObj.transform.Rotate( Vector3.right, this.currentRectXRotateSpeed);

            //Debug.Log(midiNomalizedValue);
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
        
        public void OnRippleCircleUnder()
        {
            this.director.Play(rippleCirclesTimelines[0]);
            this.SaveEffectTime("rippleCirclesUnder");
        }

        public void OnRippleCircleUpper()
        {
            this.director.Play(rippleCirclesTimelines[1]);
            this.SaveEffectTime("rippleCirclesUpper");
        }
        
        
        //
        // Visual Effect Graph
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
        
        void KeyCheck()
        {
            
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
        }
        
        
        private void OnDestroy()
        {
            StreamWriter effectTimeWritter = new StreamWriter(Application.streamingAssetsPath + "/EffectTimeline.txt",false);
            effectTimeWritter.WriteLine(effectTimelineString);
            effectTimeWritter.Flush();
            effectTimeWritter.Close();
        }
        
        void SaveEffectTime(string effectName)
        {
            //Debug.LogFormat("effectTime: {0}, effectName: {1}", this.volumetricVideoDirector.time, effectName);
            effectTimelineString += String.Format("{0},{1}\n", this.volumetricVideoDirector.time, effectName);
        }

    }
}