Shader "Custom/TwoImageColorOverlay" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AlphaTex ("Alpha Mask (A)", 2D) = "white" {}
    }
    
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200
        
        Pass
        {
            Lighting Off
            ZWrite On
            Cull Off
            Fog { Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                float4 baseCol = tex2D(_MainTex, i.uv);
                float mask = tex2D(_AlphaTex, i.uv).r;

				float influence = 1.0 - mask;

                float4 finalColor = lerp(baseCol, baseCol * _Color, influence);
                return finalColor;
            }
            ENDCG
        }
    } 
    FallBack "Transparent/Diffuse"
}