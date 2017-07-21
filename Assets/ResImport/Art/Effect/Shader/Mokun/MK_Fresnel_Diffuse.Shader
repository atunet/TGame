// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

// Simplified Diffuse shader. with fresnel in H3D2
// - HELLOHUAN
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "MoKun/Fresnel_Diffuse" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RimWidth ("Rim Width", Range(0.0,1.0)) = 0.7
        
        _RimParameter("Rim Parameter(r取0、1表示开关)", Vector) = (0,0,0,0)
    }

/// 增加alphaBlend
Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	
	
    SubShader {
        Pass {
		CGPROGRAM
			#pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };
                
                struct v2f {
                    float4 pos : SV_POSITION;
                    float4 uv : TEXCOORD0;
                    float3 color : COLOR;
                };

                uniform float4 _MainTex_ST;
                uniform float4 _RimColor;     
                
                     float _RimWidth;     
float4 _RimParameter;                     

                v2f vert (appdata_base v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos (v.vertex);
                   
                    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    float dotProduct = 1 - dot(v.normal, viewDir);

					float rimalphaPower = 1.0;
					float sinRes = abs(sin(_Time * rimalphaPower));
					float tmpPower = 2.6-sinRes*2;
					float tmpStrength = 2+sinRes*3;
					float myPower = pow( dotProduct, tmpPower ) * tmpStrength;
					o.color *= _RimColor * myPower ;

//	float sinRes = abs(sin(_Time * m_fCharacterFresnelColor.a));
//	float tmpPower = 2.6-sinRes*2;
//	float tmpStrength = 2+sinRes*3;
//	float myPower = pow(fRimPower, tmpPower ) * tmpStrength;
//	vDiffuse.xyz +=  myPower * m_fCharacterFresnelColor *  fRimAmount ;

                    // float rimWidth = 0.7;
                    //float newpower = pow(dotProduct, 5);
                    //o.color = newpower * _RimColor ;
                    o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct);
                    o.uv.zw = smoothstep(1 - _RimWidth, 1.0, dotProduct) * _RimColor.a;
                    
                    
                    //o.color = pow(o.color , 10);
                  
                    //o.color *= _RimColor * _SinTime.w ;     
					o.color *= _RimColor ;// * abs(sin(_Time.w)) ;  /// 去掉正弦波动画                

                    o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                     
                    return o;

                }

                uniform sampler2D _MainTex;
                uniform float4 _Color;

                float4 frag(v2f i) : COLOR {
                    float4 texcol = tex2D(_MainTex, i.uv.xy);
                    texcol *= _Color;
                    texcol.rgb += i.color;
                    
                    texcol.a += lerp( i.uv.a*_RimParameter.r,   0,   (texcol.a-0.1));
                                    
                    return texcol;
                }
           ENDCG
        }
    }
}

}