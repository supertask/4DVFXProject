#define PI 3.1415926


//回転
float2x2 rotate2d(float _angle){
    return float2x2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

float sdPie( in float2 p, in float2 c, in float r)
{
    p.x = abs(p.x);
    float l = length(p) - r;
    float m = length(p-c*clamp(dot(p,c),0.0,r)); // c=sin/cos of aperture
    return max(l,m*sign(c.y*p.x-c.x*p.y));
}


void SdPie_float( in float2 p, in float c, in float r, out float distance)
{
    distance = sdPie(p, c, r);
}

void SdPie2_float( in float2 p, in float t, in float r, out float distance)
{
    distance = sdPie(p, float2(sin(t),cos(t)), r);
    //distance = 0;
}

//startRadian = start radian
//radian = showing radian
void SdPieRot2_float( in float2 p, in float r, in float startRadian, in float radian, out float d0) {
    radian = fmod(radian, PI); // 0 - 360, 0 - 360, ...
    float2 c = float2(sin(radian), cos(radian));
    p = mul(p, rotate2d(PI * 0.5 - radian - startRadian));
    d0 = sdPie(p, c, r);
}