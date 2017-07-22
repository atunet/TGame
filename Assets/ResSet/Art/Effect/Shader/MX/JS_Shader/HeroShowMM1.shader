﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mx_JS/HeroShowMM1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_GlossMap("Spcular Map",2D)="black"{}
		_Tint("Spacular color Map",2D)="black"{}
		Env("Reflection Cube",Cube)="balck"{}		
		//
		intGloss("Intensity of Gloss",range(0,4))=1
		gloss("Power Index of Specular",range(1,48))=8 //高光指数
		
		intEnv("intensity of enviroment",range(0,1))=1
		evGloss("Power Index of Enviroment",range(0,1))=0 //高光指数
		evGloss2("Power Index of Enviroment 2",range(0.1,2))=1 //高光指数
		
		rimCol("Color of rim",Color)=(1,1,1,1)
		powRim("Power Index of Rim",range(1,16))=5
		intRim("Intensity of Rim",range(0,2))=1
		colorRim("Color to Rim",range(0,1))=1//Color和边缘光的混合
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass {
		Name "SHOWBACK"
		Tags{"LightMode"="ForwardBase"}
		Cull Front
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				//
				float3 normal:TEXCOORD1;
				float3 viewDir:TEXCOORD2;
				float3 vNormal:TEXCOORD3;//视空间的Normal
			};

			sampler2D _MainTex;
			sampler2D _GlossMap;
			samplerCUBE Env;
			
			float4 _MainTex_ST;

			float4 _LightColor0;
			float gloss;
			float intGloss;
			
			float4 rimCol;
			float powRim;
			float intRim;
			float colorRim;
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				//
				o.normal=v.normal;//mul(UNITY_MATRIX_MV,v.normal);
				o.viewDir=ObjSpaceViewDir(v.vertex);
				o.viewDir=mul( UNITY_MATRIX_MV,float4( o.viewDir,0)).xyz;
				
				o.vNormal=mul(unity_ObjectToWorld,float4(v.normal,0)).xyz;
				
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 col = tex2D(_MainTex, i.uv);
				clip(col.a-0.5);
				float4 env=texCUBE(Env,i.vNormal);
				half g=tex2D(_GlossMap,i.uv).r;
				g=pow(g,gloss)*intGloss;
                                
				return env*g+col;///环境*高光贴图+颜色+边缘光
	
			}
		ENDCG
	}
		Pass {
		Tags{"LightMode"="ForwardBase"}
		Cull Back
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				//
				float3 normal:TEXCOORD1;
				float3 viewDir:TEXCOORD2;
				float3 vNormal:TEXCOORD3;//视空间的Normal
			};

			sampler2D _MainTex;
			sampler2D _GlossMap;
			sampler2D _Tint;
			samplerCUBE Env;
			
			float4 _MainTex_ST;

			float4 _LightColor0;
			float gloss;
			float intGloss;
			
			float evGloss;
			float evGloss2;
			float intEnv;
			
			float4 rimCol;
			float powRim;
			float intRim;
			float colorRim;
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				//
				o.normal=v.normal;//mul(UNITY_MATRIX_MV,v.normal);
				o.viewDir=ObjSpaceViewDir(v.vertex);
				o.viewDir=mul( UNITY_MATRIX_MV,float4( o.viewDir,0)).xyz;
				
				o.vNormal=mul(unity_ObjectToWorld,float4(v.normal,0)).xyz;
				
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 col = tex2D(_MainTex, i.uv);
				clip(col.a-0.5);
				//
				float3 objN=normalize(i.normal);
				float3 viewDir=normalize(i.viewDir);
				
				
				
				//view Space Rim
				float vDotN=dot(viewDir,normalize(i.vNormal));
				//return float4( i.vNormal,1);
				vDotN=abs(vDotN);
				vDotN=max(0,vDotN);
				//
				//return vDotN;
				float g=tex2D(_GlossMap,i.uv).r;
				//return g;
				float k=pow(vDotN,gloss)*intGloss;
				
				//return k;
				//g=pow(g,gloss)*intGloss;
				//k=g*k;
				float ev=pow(vDotN,evGloss);
				float ev2=pow(g,evGloss2);
				//return ev*ev2*intEnv;
				k=k*ev2;
				
				float rim=1-vDotN;
				rim=pow(rim,powRim)*intRim;
				float4 rimC=lerp(col*rim,rim*rimCol,colorRim);
				float4 env=texCUBE(Env,i.vNormal);
				
				//float4 tint=tex2D(_GlossMap,i.uv);
				float4 tint=tex2D(_Tint,i.uv)*g*k;
				tint=tint*k*col;
				
				//return tint;
				//return env*(ev*ev2*intEnv);
				return tint+col+env*(ev*ev2*intEnv)+rimC;
	
			}
		ENDCG
	}
	} 
	FallBack "Diffuse"
}