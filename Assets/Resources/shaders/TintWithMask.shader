Shader "Custom/TintWithMask"
{
    Properties
    {
        _MainTex     ("Main Texture (RGB)", 2D) = "white" {}
        _UntintedTex ("Untinted Texture (RGB)", 2D) = "white" {}
        _Color       ("Tint Color", Color) = (1,1,1,1)
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
            sampler2D _UntintedTex;
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
                fixed4 main = tex2D(_MainTex, i.uv);
                fixed4 col  = main;

                fixed4 untinted;
                fixed4 rawU     = tex2D(_UntintedTex, i.uv);
                untinted.rgb    = rawU.rgb;
                untinted.a      = min(rawU.a, 1.0);
				
                if (untinted.a < 0.3)
                {
                    col = main * _Color;
                }
                else
                {
                    col.rgb = untinted.rgb;
                }

                return col;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}