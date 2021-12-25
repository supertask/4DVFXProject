using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AriarayDepthKitMaterialModifier : MonoBehaviour
{
    public Material mat;
    public float height = 1.8f;
    private float cachedHeight;

    // Update is called once per frame
    void Update()
    {
        if (height != cachedHeight) {
            this.mat.SetFloat("DissolveHeight", height);
            cachedHeight = height;
        }
    }
}
