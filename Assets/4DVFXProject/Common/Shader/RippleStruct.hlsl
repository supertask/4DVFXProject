#ifndef RIPPLE_STRUCT
#define RIPPLE_STRUCT

struct ParticleData
{
	int isActive;      // 有効フラグ
	float3 position;    // 座標
    float3 velocity;    // 加速度
    float4 color;       // 色
    float duration;     // 生存時間
	float scale;        // サイズ
};

#endif