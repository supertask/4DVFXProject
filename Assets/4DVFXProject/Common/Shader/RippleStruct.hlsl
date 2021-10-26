#ifndef RIPPLE_STRUCT
#define RIPPLE_STRUCT

struct ParticleData
{
	int isActive;      // 有効フラグ
    int pid;       // particle id
	float3 position;    // 座標
    float3 velocity;    // 速度
    float3 force;    // 力
    float4 color;       // 色
    //float duration;     // 生存時間
    float age; //年齢
    float lifetime; //寿命
	float scale;        // サイズ
};

#endif