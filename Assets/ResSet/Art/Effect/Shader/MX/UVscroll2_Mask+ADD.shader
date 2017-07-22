// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Mx/UVscroll2_Mask+ADD" {
Properties {
	_MainTex ("1st layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_ScrollX ("1st layer Scroll speed X", Float) = 0.0
	_ScrollY ("1st layer Scroll speed Y", Float) = 0.0
	_Scroll2X ("2nd layer Scroll speed X", Float) = 0.0
	_Scroll2Y ("2nd layer Scroll speed Y", Float) = 0.0
	//_SineAmplX ("1st layer sine amplitude X",Float) = 0.5 
	//_SineAmplY ("1st layer sine amplitude Y",Float) = 0.5
	//_SineFreqX ("1st layer sine freq X",Float) = 10 
	//_SineFreqY ("1st layer sine freq Y",Float) = 10
	//_SineAmplX2 ("2nd layer sine amplitude X",Float) = 0.5 
	//_SineAmplY2 ("2nd layer sine amplitude Y",Float) = 0.5
	//_SineFreqX2 ("2nd layer sine freq X",Float) = 10 
	//_SineFreqY2 ("2nd layer sine freq Y",Float) = 10


	//_sweepVelX("SweepVel X",Float)=0
	//_sweepGapX("SweepGap X",Float)=0
	//_sweepVelY("SweepVel Y",Float)=0
	//_sweepGapY("SweepGap Y",Float)=0

	//_sweepVelX2("2SweepVel X",Float)=0
	//_sweepGapX2("2SweepGap X",Float)=0
	//_sweepVelY2("2SweepVel Y",Float)=0
	//_sweepGapY2("2SweepGap Y",Float)=0


	_Color("Color", Color) = (1,1,1,1)
	
	_MMultiplier ("Layer Multiplier", Float) = 1.0
}

	
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	Blend One One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	LOD 100
	
	
	CGINCLUDE
	#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	//#pragma exclude_renderers molehill    
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;

	float4 _MainTex_ST;
	float4 _DetailTex_ST;
	
	float _ScrollX;
	float _ScrollY;
	float _Scroll2X;
	float _Scroll2Y;
	float _MMultiplier;
	
	//float _SineAmplX;
	//float _SineAmplY;
	//float _SineFreqX;
	//float _SineFreqY;

	//float _SineAmplX2;
	//float _SineAmplY2;
	//float _SineFreqX2;
	//float _SineFreqY2;

	//float _sweepVelX;
	//float _sweepGapX;
	//float _sweepVelY;
	//float _sweepGapY;


	//float _sweepVelX2;
	//float _sweepGapX2;
	//float _sweepVelY2;
	//float _sweepGapY2;


	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		fixed4 color : TEXCOORD1;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
		o.uv.zw = TRANSFORM_TEX(v.texcoord.xy,_DetailTex) + frac(float2(_Scroll2X, _Scroll2Y) * _Time);

		//float dx1 = (frac(_Time.y*_sweepVelX)*2-1)*_sweepGapX;
		//float dy1 = (frac(_Time.y*_sweepVelY)*2-1)*_sweepGapY;

		
		//float dx2 = (frac(_Time.y*_sweepVelX2)*2-1)*_sweepGapX2;
		//float dy2 = (frac(_Time.y*_sweepVelY2)*2-1)*_sweepGapY2;

		//o.uv.x += sin(_Time * _SineFreqX) * _SineAmplX+dx1;
		//o.uv.y += sin(_Time * _SineFreqY) * _SineAmplY+dy1;
		
		//o.uv.z += sin(_Time * _SineFreqX2) * _SineAmplX2+dx2;
		//o.uv.w += sin(_Time * _SineFreqY2) * _SineAmplY2+dy2;
		
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
			fixed4 tex2 = tex2D (_DetailTex, i.uv.zw);
			tex2.xyz*=tex2.w;
			o = tex * tex2 * i.color;
			o.rgb*=i.color.a;
			return o;
		}
		ENDCG 
	}	
}
}
