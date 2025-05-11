Shader "Custom/RBGAlphaOverlay" {
    Properties {
        _MainTex ("Main Texture (RGB)", 2D) = "white" {}
        _CutoutTex ("Cutout Texture (A)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="TransparentCutout"
            "IgnoreProjector"="True"
        }
        
        Lighting Off
        Cull Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            uniform sampler2D _MainTex;
            uniform sampler2D _CutoutTex;
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _Cutoff;
            
            v2f vert (appdata v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            half4 frag (v2f i) : COLOR {
                half4 col = tex2D(_MainTex, i.uv);
                half cutout = tex2D(_CutoutTex, i.uv).r;
                
                col.a = step(_Cutoff, cutout) * _Color.a;
                
                col.rgb *= _Color.rgb;
                
                clip(col.a - 0.001);
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/VertexLit" 
}