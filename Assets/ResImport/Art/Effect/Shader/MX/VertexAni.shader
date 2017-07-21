// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mx/VertexAni" {
Properties {
            _Color("Main color",color) = (1,1,1,1)
                _MainTex ("Base (RGB)", 2D) = "white" {}
                _Tex ("light (RGB)", 2D) = "white" {}
                _frequency ("frequency", float) = 1
                _offset ("_offset", float) = 1
                _Strength ("_Strength", float) = 1
                _Pos("pos center",Vector) =(0,0,0,0)
        }
SubShader {
Pass {
                                                  cull off
                                                Tags {"Queue" = "Grometry"}
                                                zwrite on
                                                CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members other)
#pragma exclude_renderers xbox360
                                                #pragma vertex vert
                                                #pragma fragment frag
                                                #include "UnityCG.cginc"
                                 
                                                sampler2D _MainTex;
                                                sampler2D _Tex;
                                                half4 _Color;
                                                float4 _Pos;
                                                float _frequency;
                                                float _offset;
                                                float _Strength;

                                                struct v2f {
                                                float4 pos : SV_POSITION;
                                                float4 other:TEXCOORD2;
                                                };

                                                v2f vert (appdata_full v)
                                                 {
                                            v2f o;

                                                float dis = distance (v.vertex ,_Pos) ;
                                                v.vertex.y = sin(dis + _Time.y)*_Pos.w;
                                                o.other.x = v.vertex.y ;
                                                o.pos = UnityObjectToClipPos( v.vertex );
                                            return o;
                                                }  
                 
                                                                
                                                half4 frag( v2f i ) : COLOR
                                                 {
                                                    return _Color* i.other.x; 
                                                }
                                                ENDCG 
}
}
}