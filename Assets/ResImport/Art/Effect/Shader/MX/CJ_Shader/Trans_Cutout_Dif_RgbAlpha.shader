Shader "Mx_CJ/Trans_Cutout_Dif_RgbAlpha" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_MainTexAlpha ("Base Alpha (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	//Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _MainTexAlpha;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 cAlpha = tex2D(_MainTexAlpha, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = cAlpha.r;
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}