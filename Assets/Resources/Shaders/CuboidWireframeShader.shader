﻿Shader "这就是IB/CuboidWireframeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Thickness ("Thickness", Range (0, 10)) = 3
        
        // temporal
        _ToonColor("Toon Color", color) = (1,1,1,1)
		_ToonLevel("Toon Level", range(0,1)) = 0.5
		_ToonSteps("Toon Steps", range(0,9)) = 3
    }
    
    SubShader
    {
        Pass
        {
            Tags {
                "RenderType"="Opaque"
                "Queue"="Geometry"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
            CGPROGRAM

            #include "UnityCG.cginc"
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            
            float _Thickness;
            float4 _Color;
            float4 _MainTex_ST;
            sampler2D _MainTex;
            
            // temporal
            float4 _LightColor0;
		    float4 _ToonColor;
		    float _ToonSteps;
		    float _ToonLevel;
            
            struct v2g {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                
                // temporal
                float3 lightDir : TEXCOORD1;
		    	float3 viewDir : TEXCOORD2;
		    	float3 normal : TEXCOORD3;
            };
            
            struct g2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 dist : TEXCOORD1;
                
                // temporal
                float3 lightDir : TEXCOORD2;
		    	float3 viewDir : TEXCOORD3;
		    	float3 normal : TEXCOORD4;
            };
            
            v2g vert(appdata_base v) {
                v2g o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                
                // temporal
                o.normal=v.normal;
		    	o.lightDir=ObjSpaceLightDir(v.vertex);
		    	o.viewDir=ObjSpaceViewDir(v.vertex);
                return o;
            }
            
            [maxvertexcount(3)]
            void geom(triangle v2g p[3], inout TriangleStream<g2f> triStream) {
                v2g np[3];
                bool find = false;
                for (int i = 0; i < 3 && !find; i++) {
                    for (int j = i + 1; j < 3 && !find; j++) {
                        if (abs(p[i].uv.x - p[j].uv.x) > 0.01f && abs(p[i].uv.y - p[j].uv.y) > 0.01f) {
                            np[0] = p[3 - i - j];
                            np[1] = p[i];
                            np[2] = p[j];
                            find = true;
                        }
                    }
                }
                
                float2 p0 = _ScreenParams.xy * np[0].pos.xy / np[0].pos.w;
                float2 p1 = _ScreenParams.xy * np[1].pos.xy / np[1].pos.w;
                float2 p2 = _ScreenParams.xy * np[2].pos.xy / np[2].pos.w;
                float2 v0 = p2 - p1;
                float2 v1 = p2 - p0;
                float2 v2 = p1 - p0;
                float area = abs(v1.x * v2.y - v1.y * v2.x);
                float dist0 = area / length(v0);
                float dist1 = area / length(v1);
                float dist2 = area / length(v2);
                
                g2f pIn;
                
                // temporal
                pIn.lightDir = np[0].lightDir;
                pIn.viewDir = np[0].viewDir;
                pIn.normal = np[0].normal;
                
                pIn.pos = np[0].pos;
                pIn.uv = np[0].uv;
                pIn.dist = float3(dist0, 0, 0);
                triStream.Append(pIn);
                
                // temporal
                pIn.lightDir = np[1].lightDir;
                pIn.viewDir = np[1].viewDir;
                pIn.normal = np[1].normal;
                
                pIn.pos = np[1].pos;
                pIn.uv = np[1].uv;
                pIn.dist = float3(0, dist1, 0);
                triStream.Append(pIn);
                
                // temporal
                pIn.lightDir = np[2].lightDir;
                pIn.viewDir = np[2].viewDir;
                pIn.normal = np[2].normal;
                
                pIn.pos = np[2].pos;
                pIn.uv = np[2].uv;
                pIn.dist = float3(0, 0, dist2);
                triStream.Append(pIn);
            }

            float4 frag(g2f input) : COLOR {
                float4 color = _Color * tex2D(_MainTex, input.uv);
                
                // temporal
		    	float3 N=normalize(input.normal);
		    	float3 viewDir=normalize(input.viewDir);
		    	float3 lightDir=normalize(input.lightDir);
		    	float diff=max(0,dot(N,input.lightDir));//求漫反射颜色
		    	diff=(diff+1)/2;//做亮化处理
		    	diff=smoothstep(0,1,diff);//使颜色平滑的在[0,1]范围之内
		    	float toon=floor(diff*_ToonSteps)/_ToonSteps;//把颜色做离散化处理，把diffuse颜色限制在_ToonSteps种
		    	diff=lerp(diff,toon,_ToonLevel);//调节比重
		    	float3 transColor=_ToonColor*_LightColor0*(diff);//颜色混合
		    	
                color = (_Thickness > input.dist.y || _Thickness  > input.dist.z) ? color : float4(transColor, _ToonColor.a);
                
                // temporal
                if(color.a == 0) discard;
                return color;
            }

            ENDCG
        }
    }
}