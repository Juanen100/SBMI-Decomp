Shader "Custom/TwoImageWithMask"
{
    Properties
    {
        _MainTex  ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Mask     ("Mask Offset", Float) = 0
        _Color    ("Main Color", Color) = (1,1,1,1)
        _AlphaTex ("Alpha Mask (A)", 2D) = "white" {}
    }

    SubShader
    {
        LOD 100
        Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   2.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4    _MainTex_ST;
            fixed4    _Color;
            float     _Mask;

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
                fixed4 main  = tex2D(_MainTex,  i.uv);
                fixed4 alpha = tex2D(_AlphaTex, i.uv);

                fixed4 retColor;
                retColor.rgb = main.rgb * ((_Color * (1.0 - alpha.g) + alpha.g) * main.a).rgb;
                retColor.a   = main.a * _Color.a;

                float mask = (float)(i.uv.y >= _Mask);
                return main * mask * retColor;
            }
            ENDCG
        }
    }
}