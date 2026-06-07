Shader "Custom/RGBAlphaOverlay_Outline"
{
    Properties
    {
        _MainTex          ("Main Texture (RGB)", 2D) = "white" {}
        _AlphaMap         ("Alpha (A)", 2D) = "white" {}
        _Color            ("Tint Color", Color) = (1,1,1,1)
        _OutlineIntensity ("Outline Intensity", Range(0,5)) = 2
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
            fixed4    _Color;
            fixed     _OutlineIntensity;

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
                fixed4 main = tex2D(_MainTex, i.uv) * i.color;

                fixed4 col;
                col.rgb = main.rgb;
                col.a   = tex2D(_AlphaMap, i.uv).g;
                col.g = main.g + (_OutlineIntensity * (1.0 - col.a));

                return col * _Color;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}