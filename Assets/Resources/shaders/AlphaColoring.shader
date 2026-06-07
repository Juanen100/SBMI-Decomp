Shader "Custom/AlphaColoring"
{
    Properties
    {
        _MainTex ("Alpha (A)", 2D) = "white" {}
        _Color   ("Color 1", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="true" "RenderType"="TransparentCutout" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater 0.01
            ColorMask RGB

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   2.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4    _MainTex_ST;
            fixed4    _Color;

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv  = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (col.a < 0.01)
                    discard;

                if (col.a < 0.95)
                {
                    col.rgb = col.rgb * _Color.rgb;
                    col.a   = col.a / 0.95;
                }

                return col;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}