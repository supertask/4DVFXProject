Shader "AR/OcclusionDesk"
{
    Properties
    {
        [NoScaleOffset] _ARBackgroundTexture("Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //Tags { "Queue" = "Geometry-1" }
        //ZWrite On
        //ZTest LEqual
        //ColorMask 0

        Pass
        {
            HLSLINCLUDE
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            TEXTURE2D_X(_ARBackgroundTexture);

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
                return output;
            }
            
            float4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                int2 positionSS = input.texcoord * _ScreenSize.xy;
                float4 color = LOAD_TEXTURE2D_X(_ARBackgroundTexture, positionSS);
                
                return color.r;
                //return float4(0.0, 0.0, 0.0, 0.0);
            }
            
            ENDHLSL
        }
    }
}