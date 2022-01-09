//using System.Collections;

using UnityEngine;
using UnityEngine.VFX;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.HighDefinition;

using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace VFXProject4D
{

    [RequireComponent(typeof(PlayableDirector))]

    public class EffectTrigger : MonoBehaviour // : SingletonMonoBehaviour<ImageEffectManager>
    {
        #region public

        [SerializeField] private KeyCode triangle1Key = KeyCode.Alpha1;
        [SerializeField] private KeyCode triangle2Key = KeyCode.Alpha2;
        [SerializeField] private KeyCode rect1Key = KeyCode.Alpha6;
        [SerializeField] private KeyCode rect2Key = KeyCode.Alpha7;
        [SerializeField] private KeyCode flameV1Key = KeyCode.Q;
        [SerializeField] private KeyCode flameV2Key = KeyCode.W;
        [SerializeField] private KeyCode midiTwistKey = KeyCode.E;
        [SerializeField] private KeyCode noiseDistortionKey = KeyCode.R;
        [SerializeField] public VisualEffect flameV1;
        [SerializeField] public VisualEffect flameV2;
        [SerializeField] private TimelineAsset[] triangleTimelines;
        [SerializeField] private TimelineAsset[] midiTwistTimelines;
        [SerializeField] private TimelineAsset[] noiseDistortionTwistTimelines;
        private PlayableDirector director;
        

        #endregion

        void KeyCheck()
        {
            //Triangle
            if (Input.GetKeyDown(triangle1Key))
            {
                this.director.Play(triangleTimelines[0]); 
            }
            if (Input.GetKeyDown(triangle2Key))
            {
                this.director.Play(triangleTimelines[1]); 
            }

            //Rect
            if (Input.GetKeyDown(rect2Key))
            {
                //director.Play(timelines[1]); 
            }

            //Flame V1
            if (Input.GetKeyDown(flameV1Key))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.flameV1.SendEvent("StopFlameV1");
                }
                else
                {
                    this.flameV1.SendEvent("StartFlameV1");
                }
            }

            //Flame V2
            if (Input.GetKeyDown(flameV2Key))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.flameV2.SendEvent("StopFlameV2");
                }
                else
                {
                    this.flameV2.SendEvent("StartFlameV2");
                }
            }
            

            //MIDI Twist
            if (Input.GetKeyDown(midiTwistKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    //Stop
                    Debug.Log("Stop midi twist");
                    this.director.Play(midiTwistTimelines[1]);
                }
                else
                {
                    //Start
                    Debug.Log("Start midi twist");
                    this.director.Play(midiTwistTimelines[0]);
                }
            }

            //Noise distortion
            if (Input.GetKeyDown(noiseDistortionKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.director.Play(noiseDistortionTwistTimelines[1]);
                }
                else
                {
                    this.director.Play(noiseDistortionTwistTimelines[0]);
                }
            }

        }

        private void Start()
        {
            this.director = this.GetComponent<PlayableDirector>();        
        }

        private void Update()
        {
            this.KeyCheck();
        }
        
        private void OnDestroy()
        {
        }
    }
}