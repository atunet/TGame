Shader "Mx_JS/RimLight_Outline_SD" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth ("Outline Width", Range(0.0,0.5)) = 0.035
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_Cutoff ("Cutoff", Range (0,0.9)) =0.5
		_SDColor("SD Color",Color)=(0,0,0,1)
		NBlendV("Blend of Normal Vertetx Dir",range(0,1))=0
		_Height("Height of shader", float) = 0 
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
		float4 color : COLOR;
	};
	
	sampler2D _MainTex;
	fixed4 _Color;
	fixed4 _SDColor;
	fixed4 _OutlineColor;
	fixed _OutlineWidth;
	fixed _Cutoff;
	float NBlendV;
	float4x4 _xProjector;
	float4x4 _xProjectorClip;
	float4 _fwd;
	float _Height; 

	ENDCG

	SubShader {
	
		Pass {
			//Tags { "Queue" = "Transparent  +10" "RenderType" = "Transparent"}  
			Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

			Name "OUTLINE"
	        Cull Front  
	        ZWrite On  	        
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 dir=lerp(v.normal,normalize(v.vertex.xyz/v.vertex.w),NBlendV);
				float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, dir);
				//float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);

				o.pos.xy += offset *_OutlineWidth;
				o.color = _OutlineColor;				
				o.uv = v.texcoord.xy;
				return o;
			}
			half4 frag(v2f i) :COLOR 
			{ 
				fixed4 texcol = tex2D(_MainTex, i.uv);
				//clip(texcol.a - _Cutoff);
				fixed4 c = i.color;
				c.a = 0;
				return c; 
			}
			ENDCG
		}
				
		
		Pass
		{
			Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			Name "BASE"

			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				
				//无光照
				o.color = (1,1,1,1);
				
				//漫反射
				/*
				float4x4 modelMatrix = _Object2World;
				float4x4 modelMatrixInverse = _World2Object;
				float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse)));
				float3 lightDirection = normalize(float3(_WorldSpaceLightPos0));
				float3 diffuseReflection = (1, 1, 1) * float3(_Color) * max(0.0, dot(normalDirection, lightDirection));
				o.color = float4(diffuseReflection, 1.0) * 2;
				*/
				return o;
			}

			fixed4 frag(v2f i) : COLOR 
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				//clip(texcol.a - _Cutoff);
				texcol *= i.color;
				return texcol;
			}

			ENDCG
		}
		
		Pass {
		Name "MObaCaster"
		//Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "LightMode" = "ForwardBase" }
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		//Blend SrcAlpha OneMinusSrcAlpha
		//Tags { "LightMode" = "ShadowCaster" }
		//Offset 1, 1
		
		Fog {Mode Off}
		ZWrite On 
		ZTest LESS
		Cull Back
		Cull Front

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		//#pragma multi_compile_shadowcaster
		#include "UnityCG.cginc"

		//struct v2fs { 
	//		V2F_SHADOW_CASTER;
	//		float2  uv : TEXCOORD1;
	//	};
		struct v2fs {
				float4 pos:POSITION;
				float2 uv:TEXCOORD1;
			};

		uniform float4 _MainTex_ST;
		float4 _clip(float4 vert)
		{
			float3 z = float3(0,0.5,0.2);//_fwd.xyz;
			float3 pnrm = float3(0,1,0);
			float pd = 0.0;//0.1f;
	        float invZdotN = 1.0f / dot(z, pnrm);
            float3 v = vert - (invZdotN * (pd + dot(vert, pnrm))) * z+float3(0,0.08f,0);
			v.g = v.g + _Height;
			return float4(v,1);
		}
		v2fs vert( appdata_base v )
		{
			v2fs o;
			//TRANSFER_SHADOW_CASTER(o)
			float4 cv = mul(unity_ObjectToWorld,v.vertex);
			
			float4 vv  = mul(unity_WorldToObject,_clip(cv)); 

			o.pos = UnityObjectToClipPos(vv);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

		//uniform sampler2D _MainTex; 
		//uniform fixed _Cutoff;
		//uniform fixed4 _Color;

		float4 frag( v2fs i ) : COLOR
		{
			//fixed4 texcol = tex2D( _MainTex, i.uv );
			//clip( texcol.a*_Color.a - _Cutoff );  
			//clip( texcol.a - _Cutoff );
			//SHADOW_CASTER_FRAGMENT(i)
			//texcol.xyz=float(0.5f).xxx;
			return _SDColor;//float4(0.0f,0.0f,0.0f,0.0f);//float4(0f,0f,0f,0.5f);//texcol;
		}
		ENDCG
		}
		
	}
	
	//Fallback "Toon/Basic"
}
