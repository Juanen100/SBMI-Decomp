Shader "Custom/RGBAlphaOverlay (HardClip)" {
Properties {
 _MainTex ("Main Texture  (RGB)", 2D) = "" {}
 _AlphaMap ("Alpha  (A)", 2D) = "" {}
 _Color ("Tint Color", Color) = (1,1,1,1)
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}