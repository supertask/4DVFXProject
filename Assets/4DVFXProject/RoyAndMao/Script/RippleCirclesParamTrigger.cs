using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class RippleCirclesParamTrigger : MonoBehaviour
{
    public Vector2 visualizingHeightRange = new Vector2(-0.55f, 0.65f);
    public Vector3 ellipseCenter = new Vector3(0, 0.0f, 0);
    public float ellipseRadiusY = 1.5f;
    public float ellipseRadiusX = 2.5f;

    public bool isUpper = false;

    private List<VisualEffect> vfxList;
    
    void InitVFX()
    {
        this.vfxList = new List<VisualEffect>();
        foreach (Transform rippleCircleTransform in this.transform)
        {
            vfxList.Add(
                rippleCircleTransform.gameObject.GetComponent<VisualEffect>()
            );
        }
    }

    void OnValidate()
    {
        if (vfxList == null) { InitVFX(); }

        //Debug.Log("onvalidate");
        foreach(VisualEffect vfx in vfxList)
        {
            vfx.SetVector2("VisualizingHeightRange", visualizingHeightRange);
            vfx.SetVector3("EllipseCenter", ellipseCenter);
            vfx.SetBool("IsUpper", this.isUpper);
            vfx.SetFloat("EllipseRadiusX", ellipseRadiusX);
            vfx.SetFloat("EllipseRadiusY", ellipseRadiusY);
        }
    }
}
