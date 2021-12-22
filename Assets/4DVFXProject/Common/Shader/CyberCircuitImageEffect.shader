Shader "Hidden/CyberCircuitImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Noise/Noise.hlsl"
            #include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Noise/ChebyshevDistanceVoronoi3dSG.hlsl"
            #include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Noise/WaveSG.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _Color;
            float _WaveScale;
            float _DissolvePercentage;
            float _LineWidth;
            float _LineColor;

            fixed4 frag (v2f IN) : SV_Target
            {
                _DissolvePercentage = 0.35;
                _LineWidth = 0.02;
                _LineColor = float4(1,0,0,1);
                
                float grayColor;
                float4 distance;
                ChebyshevDistanceVoronoi3D_float(
                    IN.uv * 20 + 0.5, 0, 2, _Time.x * 0.1,
                    grayColor, distance);
                float res;
                Wave1_float(float2(distance.x * _WaveScale, 0), 0, res);
                float cyberCircuit = (res > 0.99 ? 1 : 0);
                
                float clipping = fbm2DWithMorgan(IN.uv * 3, 2) - _DissolvePercentage;
                if (0 < clipping && clipping < _LineWidth && _DissolvePercentage > 0) {
                    return _LineColor; 
                }
                else if (_LineWidth < clipping) {
                    //base color
                    return float4(0,0,0,1);
                }
                else if (clipping < 0) {
                    return cyberCircuit;
                    //return float4(0,0,0,0);
                    //builtinData.emissiveColor = wireframeSampleTex * lerp(wireframeColor, float4(0,0,0,0), wireframeAlpha);
                }
                
                return float4(0,0,0,0);
                
                //return cyberCircuit;
                
                //return res;
                //return _Color * (res < 0.99 ? 1 : 0);

                //return float4(grayColor, grayColor, grayColor, 1);
                //fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                //return col;
            }
            ENDCG
        }
    }
}
