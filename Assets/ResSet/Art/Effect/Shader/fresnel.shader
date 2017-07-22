// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-1427-OUT,clip-2293-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32135,y:32598,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32135,y:32818,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32294,y:33378,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_If,id:2293,x:32085,y:33101,varname:node_2293,prsc:2|A-5568-OUT,B-9049-A,GT-8835-OUT,EQ-8835-OUT,LT-673-OUT;n:type:ShaderForge.SFN_If,id:3006,x:32085,y:33278,varname:node_3006,prsc:2|A-5419-OUT,B-9049-A,GT-8835-OUT,EQ-8835-OUT,LT-673-OUT;n:type:ShaderForge.SFN_Vector1,id:8835,x:31871,y:33235,varname:node_8835,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:673,x:31871,y:33314,varname:node_673,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:9049,x:31356,y:33144,ptovrint:False,ptlb:node_9049,ptin:_node_9049,varname:node_9049,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f0d5b8ef266133f4ba905c8661f439c9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Subtract,id:5419,x:31693,y:33302,varname:node_5419,prsc:2|A-5568-OUT,B-9119-OUT;n:type:ShaderForge.SFN_Vector1,id:9119,x:31506,y:33368,varname:node_9119,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Abs,id:5568,x:31616,y:33082,varname:node_5568,prsc:2|IN-2053-A;n:type:ShaderForge.SFN_Subtract,id:8607,x:32309,y:33210,varname:node_8607,prsc:2|A-2293-OUT,B-3006-OUT;n:type:ShaderForge.SFN_Multiply,id:1373,x:32495,y:33210,varname:node_1373,prsc:2|A-8607-OUT,B-797-RGB;n:type:ShaderForge.SFN_Add,id:1427,x:32515,y:32793,varname:node_1427,prsc:2|A-3926-OUT,B-1373-OUT;n:type:ShaderForge.SFN_Multiply,id:3926,x:32321,y:32704,varname:node_3926,prsc:2|A-6074-RGB,B-2053-RGB,C-4904-OUT;n:type:ShaderForge.SFN_Fresnel,id:4904,x:32135,y:32432,varname:node_4904,prsc:2;proporder:6074-797-9049;pass:END;sub:END;*/

Shader "Shader Forge/fresnel" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _node_9049 ("node_9049", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _node_9049; uniform float4 _node_9049_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float node_5568 = abs(i.vertexColor.a);
                float4 _node_9049_var = tex2D(_node_9049,TRANSFORM_TEX(i.uv0, _node_9049));
                float node_2293_if_leA = step(node_5568,_node_9049_var.a);
                float node_2293_if_leB = step(_node_9049_var.a,node_5568);
                float node_673 = 0.0;
                float node_8835 = 1.0;
                float node_2293 = lerp((node_2293_if_leA*node_673)+(node_2293_if_leB*node_8835),node_8835,node_2293_if_leA*node_2293_if_leB);
                clip(node_2293 - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_3006_if_leA = step((node_5568-0.2),_node_9049_var.a);
                float node_3006_if_leB = step(_node_9049_var.a,(node_5568-0.2));
                float3 emissive = ((_MainTex_var.rgb*i.vertexColor.rgb*(1.0-max(0,dot(normalDirection, viewDirection))))+((node_2293-lerp((node_3006_if_leA*node_673)+(node_3006_if_leB*node_8835),node_8835,node_3006_if_leA*node_3006_if_leB))*_TintColor.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
