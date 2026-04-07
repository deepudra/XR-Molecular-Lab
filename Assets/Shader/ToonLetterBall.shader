Shader "Cartoon/Atom"
{
    Properties
    {
        [Header(Base Settings)]
        _Color ("Ball Color", Color) = (1, 1, 1, 1)
        _LetterTex ("Letter Texture (Alpha)", 2D) = "white" {}
        _LetterColor ("Letter Color", Color) = (0, 0, 0, 1)
        
        [Header(Toon Settings)]
        _Step1 ("Toon Step 1", Range(0, 1)) = 0.4
        _Step2 ("Toon Step 2", Range(0, 1)) = 0.8
        
        [Header(Projection Settings)]
        _LetterScale ("Letter Scale", Range(0.1, 5)) = 1.8
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        Pass
        {
            Name "ForwardLit"
            ZWrite On
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD0;
                float3 localPos   : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _LetterColor;
                float _Step1;
                float _Step2;
                float _LetterScale;
            CBUFFER_END

            sampler2D _LetterTex;

            Varyings vert (Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.localPos = input.positionOS.xyz;

                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float3 lightDir = normalize(_MainLightPosition.xyz);
                float dN = dot(normalize(input.normalWS), lightDir);

                float toon = dN > _Step2 ? 1.0 :
                             dN > _Step1 ? 0.7 :
                             0.5;

                float3 worldBallPos = TransformObjectToWorld(float3(0,0,0));
                float3 viewDirWS = GetCameraPositionWS() - worldBallPos;
                float3 viewDirLoc = normalize(mul((float3x3)GetWorldToObjectMatrix(), viewDirWS));

                float3 up = float3(0,1,0);
                float3 right = normalize(cross(up, viewDirLoc));
                up = cross(viewDirLoc, right);

                float2 uv = float2(-dot(input.localPos, right), dot(input.localPos, up));
                uv = uv * _LetterScale + 0.5;

                float mask = smoothstep(0.0, 0.2, dot(normalize(input.localPos), viewDirLoc));

                half4 tex = tex2D(_LetterTex, uv);
                float alpha = tex.a * mask;

                half3 finalBase = _Color.rgb * toon;
                half3 finalCol = lerp(finalBase, _LetterColor.rgb, alpha);

                return half4(finalCol, 1.0);
            }

            ENDHLSL
        }

    }
}