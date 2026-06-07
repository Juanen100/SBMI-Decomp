Shader "Custom/RGBAlphaOverlay (HardClip)"
{
    Properties
    {
        _MainTex  ("Main Texture (RGB)", 2D) = "white" {}
        _AlphaMap ("Alpha (A)", 2D) = "white" {}
        _Color    ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater 0.01
            ColorMask RGB

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   2.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaMap;
            float4    _MainTex_ST;
            fixed4    _Color;

            struct appdata
            {
                float4 vertex   : POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv    : TEXCOORD0;
                float2 uv1   : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.uv    = v.texcoord.xy;
                o.uv1   = (v.vertex.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 absuv1 = abs(i.uv1);
                float  border = 1.0 - max(absuv1.x, absuv1.y);
                if (border < 0.0)
                    discard;

                fixed4 col;
                col.rgb = (tex2D(_MainTex, i.uv) * i.color).rgb;
                col.a   = tex2D(_AlphaMap, i.uv).g;
                return col * _Color;
            }
            ENDCG
        }
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater 0.01
            ColorMask RGB
            ColorMaterial AmbientAndDiffuse
            SetTexture [_MainTex]  { combine texture, texture alpha }
            SetTexture [_AlphaMap] { combine previous, texture alpha }
        }
    }
}