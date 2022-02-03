using Unity.Mathematics;
using UnityEngine;

namespace VFXProject4D
{
    static class Util
    {
        public static float Remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
