// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Mx/UI_Blend_NoAlpha" 
{
Properties 
{
	_MainTex ("Particle Texture", 2D) = "black" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_OffsetFactor("OffsetFactor", Int) = -1
	_OffsetUnit("OffsetUnit", Int) = -30000
}

SubShader 
{
	Tags 
	{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
		"PreviewType"="Plane"
	}
	
	Cull Off 
	Lighting Off 
	ZWrite Off 
	Offset  [_OffsetFactor], [_OffsetUnit]
	ZTest LEqual
	Fog { Mode Off }
	Blend SrcAlpha OneMinusSrcAlpha
	
	Pass {	
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;		
		struct appdata_t {
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		float4 _MainTex_ST;
		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			fixed4 col;
			fixed4 tex = tex2D(_MainTex, i.texcoord);
			col.rgba = i.color.rgba * tex.rgba;
			//return i.color;
			return col;
		}
		ENDCG 
	}
} 	

}
