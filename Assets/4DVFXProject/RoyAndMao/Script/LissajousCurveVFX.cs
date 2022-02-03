using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Mathematics;

namespace VFXProject4D
{
	[ExecuteInEditMode]
	public class LissajousCurveVFX : LissajousCurve
	{
		[SerializeField] public Transform attractor;
		[SerializeField] public VisualEffect vfx;
		
		[SerializeField] public bool useHeightCurve = true;
		[SerializeField] public AnimationCurve heightCurve;
		[SerializeField] public bool useFbm = true;
		[SerializeField] public float fbmPositionAmount = 1.0f;
		[SerializeField] public int fbmOctaves = 1;
        [SerializeField] public float fbmFrequency = 1;


		private MeshRenderer attractorMeshRenderer;
		private float fbmTime;
		
		
		public override void Start()
		{
			this.attractorMeshRenderer = this.attractor.gameObject.GetComponent<MeshRenderer>();
			this.attractorMeshRenderer.enabled = false;

		}
		
		public override void Update()
		{
			//this.attractor.position = SamplePosition(this.attractor.position);
			//this.vfx.SetVector3("AttractorPosition", this.attractor.position);
			this.attractor.localPosition = this.SamplePosition(this.attractor.localPosition);
			this.vfx.SetVector3("AttractorPosition", this.attractor.localPosition);
            fbmTime += UnityEngine.Time.deltaTime * fbmFrequency;
		}
		
		public override Vector3 SamplePosition(Vector3 position)
		{
			Vector3 newPosition = base.SamplePosition(position);
			
			if (useHeightCurve)
			{
				float theta = currentTheta.z;

				// back is 0 radian, front is PI radian, left = PI / 2, right = (3 * PI) / 2
				theta -= (Mathf.PI / 2.0f);
				theta %= ( 2.0f * Mathf.PI );
				
				float normalizedTheta = Util.Remap(theta, 0, 2 * Mathf.PI, 0, 1);
				float height = heightCurve.Evaluate(normalizedTheta);
				//Debug.Log("height: " + height);				
				newPosition.y *= height;
			}

			if (useFbm)
			{
				Vector3 np = new Vector3(
					Fbm(0, fbmTime, fbmOctaves),
					Fbm(0, fbmTime, fbmOctaves),
					Fbm(0, fbmTime, fbmOctaves)
				);
				np *= (fbmPositionAmount / 0.75f);
				newPosition += np;
			}
			
			return newPosition;
		}
		
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
	}
}
