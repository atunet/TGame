// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mx/SpaceDistort" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Distortion ("Distortion", Range (0.01, 0.1)) = 0.02
	}
	
	
	SubShader {
		cull off // TEST
		// We must be transparent, so other objects are drawn before this one.
		Tags {"Queue"="Overlay-8" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		LOD 100
		fog {Mode off} 
		ZWrite Off
		
		
		GrabPass {"_myGrabTexture"}
	
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
				float4 vertexColor : COLOR;
			};
			
			struct v2f {
				float4 vertex : POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvmain : TEXCOORD2;
				float4 vertexColor : COLOR;
			};
			
			float _Distortion;
			sampler2D _myGrabTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
				o.vertexColor = v.vertexColor;
				return o;
			}
			
			
			half4 frag( v2f i ) : COLOR
			{
				// calculate perturbed coordinates
				half2 bump = tex2D( _MainTex, i.uvmain ).rg; // we could optimize this by just reading the x & y without reconstructing the Z
				i.uvgrab.xy += bump*_Distortion*i.uvgrab.w*i.vertexColor.a;
				
				//half4 col = tex2D( _myGrabTexture, i.uvgrab.xy/i.uvgrab.w );
				half4 col = tex2Dproj( _myGrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				return col;
			}
			ENDCG
			
		} // of pass
		
	} // of subshader
	
	
	SubShader {
		Pass {
			ZWrite Off
			Lighting Off
		    Blend Zero One
		    SetTexture [_MainTex] { combine texture }
		}
	}
	
}
