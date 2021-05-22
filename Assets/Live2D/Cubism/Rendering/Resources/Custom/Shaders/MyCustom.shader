﻿Shader "Live2D Extension/MyCustom"
{
    Properties
    {
        [PerRendererData] _MainTex("Main Texture", 2D) = "white" {}
        [PerRendererData] cubism_ModelOpacity("Model Opacity", Float) = 1

        _SrcColor("Source Color", Int) = 0
        _DstColor("Destination Color", Int) = 0
        _SrcAlpha("Source Alpha", Int) = 0
        _DstAlpha("Destination Alpha", Int) = 0

        _Cull("Culling", Int) = 0

        [Toggle(CUBISM_MASK_ON)] cubism_MaskOn("Mask?", Int) = 0
        [Toggle(CUBISM_INVERT_ON)] cubism_InvertOn("Inverted?", Int) = 0
        [PerRendererData] cubism_MaskTexture("cubism_Internal", 2D) = "white" {}
        [PerRendererData] cubism_MaskTile("cubism_Internal", Vector) = (0, 0, 0, 0)
        [PerRendererData] cubism_MaskTransform("cubism_Internal", Vector) = (0, 0, 0, 0)

        _TexelX("Texel X", Float) = 0
        _TexelY("Texel Y", Float) = 0

        _BlendColor("Blend Color", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull     [_Cull]
        Lighting Off
        ZWrite   Off
        Blend    [_SrcColor][_DstColor], [_SrcAlpha][_DstAlpha]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile CUBISM_MASK_ON CUBISM_MASK_OFF CUBISM_INVERT_ON

            #include "UnityCG.cginc"
            #include "../../Live2D/Cubism/Shaders/CubismCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO

                CUBISM_VERTEX_OUTPUT
            };

            sampler2D _MainTex;

            CUBISM_SHADER_VARIABLES

            v2f vert (appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;

                CUBISM_INITIALIZE_VERTEX_OUTPUT(IN, OUT);
                return OUT;
            }

            float _TexelX;
            float _TexelY;
            float4 _BlendColor;

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
                OUT.rgb *= _BlendColor.rgb;
                CUBISM_APPLY_ALPHA(IN, OUT);
                return OUT;
            }
            ENDCG
        }
    }
}
