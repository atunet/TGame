// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Mx/Particles_Blend_Alpha" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
    _MMultiplier ("Layer Multiplier", Range(1, 10.0)) = 1.0
   // _InvFade ("Soft Particles Factor", Range(0.01,3)) = 1
	_Color("Color", Color) = (1,1,1,1)
}

	
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
			Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha
	
	
	LOD 100
	
	
	CGINCLUDE
	//#pragma exclude_renderers molehill    
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;
	float4 _TintColor;
	float4 _MainTex_ST;
	float4 _DetailTex_ST;
	float _MMultiplier;
	float4 _Color;
	struct appdata_t
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			/*
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		fixed4 color : TEXCOORD1;
	}; 
	*/
	
	v2f vert (appdata_t v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = TRANSFORM_TEX(v.uv.xy,_MainTex);
		o.uv.zw = TRANSFORM_TEX(v.uv.xy,_DetailTex);
		o.color = _MMultiplier * _Color * v.color;
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv.xy);
			fixed4 tex2 = tex2D (_DetailTex, i.uv.xy);
			tex.a=tex2.r;
			o = tex * i.color * _TintColor * 2.5;
			return o;
		}
		ENDCG 
	}	
}

SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse

			SetTexture[_MainTex] {
			    combine texture * primary
			}
			SetTexture [_DetailTex]
			{
				Combine previous, texture
			}


		}
	}

}
