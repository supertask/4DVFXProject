
#ifndef TWIST_POSITION
#define TWIST_POSITION


//
// Twist vertex shader
// https://joy.recurse.com/posts/174-vertex-love-online-vertex-shader-editor
//

#include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Util/Constant.hlsl"

float4x4 rotateY(float angle)
{
	float c = cos(angle);
	float s = sin(angle);
    return float4x4(
        float4(c, 0, -s, 0),
        float4(0, 1, 0, 0),
        float4(s, 0, c, 0),
        float4(0, 0, 0, 1)
	);
}

void Twist_float(
	float3 position,
	float timeScale,
	float angleScale,
	out float3 twistedPosition)
{
	float3 y = position.y + (_Time.x * timeScale);
	float4 rotatedPosition4 = mul(rotateY(y * angleScale * PI), float4(position, 1.0));
	twistedPosition = rotatedPosition4.xyz;
}


#endif