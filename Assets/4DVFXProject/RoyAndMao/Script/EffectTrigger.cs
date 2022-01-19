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

        //Shape VFX
        [SerializeField] private KeyCode triangle1Key = KeyCode.Alpha1;
        [SerializeField] private KeyCode triangle2Key = KeyCode.Alpha2;
        [SerializeField] private KeyCode rect1Key = KeyCode.Alpha6;
        [SerializeField] private KeyCode rect2Key = KeyCode.Alpha7;
        [Space]
        
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
        [SerializeField] private KeyCode noiseDistortionKey = KeyCode.X;
        [SerializeField] private KeyCode twistKey = KeyCode.C;
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

        [SerializeField] private TimelineAsset[] midiTwistTimelines;
        [SerializeField] private TimelineAsset[] twistTimelines;
        [SerializeField] private TimelineAsset[] noiseDistortionTwistTimelines;
        [SerializeField] private TimelineAsset[] warpV2Timelines;
        [SerializeField] private TimelineAsset[] gvoxelizerTimelines;

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
        
        void KeyCheck()
        {
            //Triangle
            if (Input.GetKeyDown(triangle1Key))
            {

            }
            if (Input.GetKeyDown(triangle2Key))
            {
            }

            //Rect
            if (Input.GetKeyDown(rect2Key))
            {
                //director.Play(timelines[1]); 
            }
            
            //Flame V2 VFX
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

            //Flame V1 VFX
            if (Input.GetKeyDown(flameV1Key))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.flameV1.SendEvent("StopFlameV1");
                    this.SaveEffectTime("StopFlameV1");
                }
                else
                {
                    this.flameV1.SendEvent("StartFlameV1");
                    this.SaveEffectTime("StartFlameV1");
                }
            }

            //Flame V2 VFX
            if (Input.GetKeyDown(flameV2Key))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.flameV2.SendEvent("StopFlameV2");
                    this.SaveEffectTime("StopFlameV2");
                }
                else
                {
                    this.flameV2.SendEvent("StartFlameV2");
                    this.SaveEffectTime("StartFlameV2");
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

            /*
            //Simple Twist
            if (Input.GetKeyDown(twistKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.director.Play(twistTimelines[1]);
                    this.SaveEffectTime("TwistStop");
                }
                else
                {
                    this.director.Play(twistTimelines[0]);
                    this.SaveEffectTime("TwistStart");
                }
            }

            //Noise distortion
            if (Input.GetKeyDown(noiseDistortionKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.director.Play(noiseDistortionTwistTimelines[1]);
                    this.SaveEffectTime("NoiseDistortionStop");
                }
                else
                {
                    this.director.Play(noiseDistortionTwistTimelines[0]);
                    this.SaveEffectTime("NoiseDistortionStart");
                }
            }
            */

            //Warp VFX
            if (Input.GetKeyDown(warpV2Key))
            {
                //Debug.Log("warpV2");
                this.director.Play(warpV2Timelines[0]);
                this.SaveEffectTime("WarpV2");
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