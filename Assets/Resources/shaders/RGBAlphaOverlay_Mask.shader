Shader "Custom/RGBAlphaOverlay_Mask"
{
    Properties
    {
        _MainTex  ("Main Texture (RGB)", 2D) = "white" {}
        _AlphaMap ("Alpha (A)", 2D) = "white" {}
        _Mask     ("Mask Offset", Float) = 0
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
            float     _Mask;

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
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.uv    = v.texcoord.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col;
                col.rgb = (tex2D(_MainTex, i.uv) * i.color).rgb;
                col.a   = tex2D(_AlphaMap, i.uv).g;

                float mask = (float)(i.uv.y >= _Mask);
                col *= mask;

                return col;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}