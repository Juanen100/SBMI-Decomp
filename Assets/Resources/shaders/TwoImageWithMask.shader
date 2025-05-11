Shader "Custom/TwoImageWithMask" {
Properties {
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _Mask ("Mask Offset", Float) = 0
 _Color ("Main Color", Color) = (1,1,1,1)
 _AlphaTex ("Alpha Mask (A)", 2D) = "white" {}
}
	SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        ZWrite On
        
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _AlphaTex;
        fixed4 _Color;
        float _Mask;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float2 baseUV = IN.uv_MainTex;
            float2 maskUV = baseUV + float2(_Mask, _Mask);

            fixed4 baseCol = tex2D(_MainTex, baseUV);
            float maskVal = tex2D(_AlphaTex, maskUV).r;

            float influence = 1.0 - saturate(maskVal);

            fixed3 tinted = lerp(baseCol.rgb, baseCol.rgb * _Color.rgb, influence);

            o.Albedo = tinted;
            o.Alpha = baseCol.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}