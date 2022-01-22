using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class LissajousCurveVFX : LissajousCurve
{
	[SerializeField] public Transform attractor;
	[SerializeField] public VisualEffect vfx;
    
	public override void Update()
	{
		this.attractor.position = SamplePosition(this.attractor.position);
        this.vfx.SetVector3("AttractorPosition", this.attractor.position);
	}
}