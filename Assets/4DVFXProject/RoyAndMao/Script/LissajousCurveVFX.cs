using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class LissajousCurveVFX : LissajousCurve
{
	[SerializeField] public Transform attractor;
	[SerializeField] public VisualEffect vfx;
	private MeshRenderer attractorMeshRenderer;
	
	public override void Start()
	{
		this.attractorMeshRenderer = this.attractor.gameObject.GetComponent<MeshRenderer>();
	}
    
	public override void Update()
	{
		//this.attractor.position = SamplePosition(this.attractor.position);
        //this.vfx.SetVector3("AttractorPosition", this.attractor.position);
		this.attractor.localPosition = SamplePosition(this.attractor.localPosition);
        this.vfx.SetVector3("AttractorPosition", this.attractor.localPosition);
		this.attractorMeshRenderer.enabled = false;

	}
}