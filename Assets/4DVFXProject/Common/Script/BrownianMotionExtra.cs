using Unity.Mathematics;
using UnityEngine;

namespace Klak.Motion
{
    static class UtilitiesPublic
    {
        public static Unity.Mathematics.Random Random(uint seed)
        {
            // Auto reseeding
            if (seed == 0) seed = (uint)UnityEngine.Random.Range(0, 0x7fffffff);

            var random = new Unity.Mathematics.Random(seed);

            // Abandon a few first numbers to warm up the PRNG.
            random.NextUInt();
            random.NextUInt();

            return random;
        }
    }
    
    [AddComponentMenu("Klak/Procedural Motion/Brownian Motion Extra")]
    public class BrownianMotionExtra : MonoBehaviour
    {
        #region Editable attributes

        public float3 positionAmount = 1;
        public float3 rotationAmount = 10;
        public float frequency = 1;
        [Range(0, 9)] public int octaves = 2;
        public uint seed = 0;
        public bool pause = false;

        #endregion

        #region Public method

        public void Rehash()
        {
            //Start position & rotation
            var rand = UtilitiesPublic.Random(seed);
            _positionOffset = rand.NextFloat3(-1e3f, 1e3f);
            _rotationOffset = rand.NextFloat3(-1e3f, 1e3f);

            ApplyMotion(new float3(0,0,0), new float3(0,0,0));
            //ApplyMotion(_positionOffset, _rotationOffset);
        }

        #endregion

        #region Private members

        float3 _positionOffset;
        float3 _rotationOffset;
        float _time;

        float3 _initialPosition;
        quaternion _initialRotation;
        
        private bool cachedPause = false;

        float Fbm(float x, float y, int octave)
        {
            var p = math.float2(x, y);
            var f = 0.0f;
            var w = 0.5f;
            for (var i = 0; i < octave; i++)
            {
                f += w * noise.snoise(p);
                p *= 2.0f;
                w *= 0.5f;
            }
            return f;
        }

        void ApplyMotion(float3 positionOffset, float3 rotationOffset)
        {
            var np = math.float3(
                Fbm(positionOffset.x, _time, octaves),
                Fbm(positionOffset.y, _time, octaves),
                Fbm(positionOffset.z, _time, octaves)
            );

            var nr = math.float3(
                Fbm(rotationOffset.x, _time, octaves),
                Fbm(rotationOffset.y, _time, octaves),
                Fbm(rotationOffset.z, _time, octaves)
            );

            np = np * positionAmount / 0.75f;
            nr = nr * rotationAmount / 0.75f;

            var nrq = quaternion.EulerZXY(math.radians(nr));

            transform.localPosition = _initialPosition + np;
            transform.localRotation = math.mul(nrq, _initialRotation);                

        }

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            Rehash();
        }

        void OnEnable()
        {
            _initialPosition = transform.localPosition;
            _initialRotation = transform.localRotation;
        }

        void OnDisable()
        {
            transform.localPosition = _initialPosition;
            transform.localRotation = _initialRotation;
        }

        void Update()
        {
            if (pause != cachedPause) {
                if (pause) {
                    //on start pause
                    
                } else {
                    //on end pause
                    //Debug.Log("On end pause");
                    _initialPosition = transform.localPosition;
                    _initialRotation = transform.localRotation;
                    _time = 0;
                }
            } 
            
            if (!pause) {
                _time += UnityEngine.Time.deltaTime * frequency;
                ApplyMotion(new float3(0,0,0), new float3(0,0,0));
                //ApplyMotion(_positionOffset, _rotationOffset);

            }
            cachedPause = pause;
        }

        #endregion
    }
}
