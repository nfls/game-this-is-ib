// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "这就是IB/WireframePointDissolutionShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Thickness("Thickness", Range (0, 10)) = 3
        _ToonColor("Toon Color", color) = (1,1,1,1)
		_ToonLevel("Toon Level", range(0,1)) = 0.5
		_ToonSteps("Toon Steps", range(0,9)) = 3
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0.0, 1.0)) = 0.5
		_EdgeLength("Edge Length", Range(0.0, 0.2)) = 0.1
		_StartPoint("Start Point", Vector) = (0, 0, 0, 0)
		_MaxDistance("Max Distance", Float) = 0
		_DistanceFactor("Distance Factor", Range(0.0, 1.0)) = 0.5
	}
	SubShader
	{
		Tags { "Queue"="Geometry" "RenderType"="Opaque" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
			Offset -1, 0

			CGPROGRAM

			#include "ThisIsIB.cginc"
            #pragma target 4.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

			struct v2g {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvNoiseTex : TEXCOORD1;
				float3 objPos : TEXCOORD2;
				float3 objStartPos : TEXCOORD3;
				float3 lightDir : TEXCOORD4;
				float3 normal : TEXCOORD5;
			};

			struct g2f {
				float4 pos : POSITION;
        		float2 uv : TEXCOORD0;
				float2 uvNoiseTex : TEXCOORD1;
				float3 objPos : TEXCOORD2;
				float3 objStartPos : TEXCOORD3;
        		float3 dist : TEXCOORD4;
        		float3 lightDir : TEXCOORD5;
				float3 normal : TEXCOORD6;
			};

			fixed4 _EdgeColor;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _Threshold;
			float _EdgeLength;
			float _MaxDistance;
			float4 _StartPoint;
			float _DistanceFactor;
			
			v2g vert (appdata_base v) {
				v2g o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uvNoiseTex = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				o.objPos = v.vertex;
				o.objStartPos = mul(unity_WorldToObject, _StartPoint);
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.normal = v.normal;
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

        		pIn.lightDir = np[0].lightDir;
        		pIn.normal = np[0].normal;
        		pIn.pos = np[0].pos;
        		pIn.uv = np[0].uv;
        		pIn.dist = float3(dist0, 0, 0);
				pIn.uvNoiseTex = np[0].uvNoiseTex;
				pIn.objPos = np[0].objPos;
				pIn.objStartPos = np[0].objStartPos;
        		triStream.Append(pIn);

        		pIn.lightDir = np[1].lightDir;
        		pIn.normal = np[1].normal;
        		pIn.pos = np[1].pos;
        		pIn.uv = np[1].uv;
        		pIn.dist = float3(0, dist1, 0);
				pIn.uvNoiseTex = np[1].uvNoiseTex;
				pIn.objPos = np[1].objPos;
				pIn.objStartPos = np[1].objStartPos;
        		triStream.Append(pIn);

        		pIn.lightDir = np[2].lightDir;
        		pIn.normal = np[2].normal;
        		pIn.pos = np[2].pos;
        		pIn.uv = np[2].uv;
        		pIn.dist = float3(0, 0, dist2);
				pIn.uvNoiseTex = np[2].uvNoiseTex;
				pIn.objPos = np[2].objPos;
				pIn.objStartPos = np[2].objStartPos;
        		triStream.Append(pIn);
			}
			
			fixed4 frag (g2f i) : SV_Target {
				float dist = length(i.objPos.xyz - i.objStartPos.xyz);
				float normalizedDist = saturate(dist / _MaxDistance);

				fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r * (1 - _DistanceFactor) + normalizedDist * _DistanceFactor;
				clip(cutout - _Threshold);

				fixed4 color = getcolor(i.pos, i.uv, i.dist, i.lightDir, i.normal);

				float degree = saturate((cutout - _Threshold) / _EdgeLength);
				fixed4 finalColor = lerp(_EdgeColor, color, degree);
				return fixed4(finalColor.rgb, 1);
			}

			ENDCG
		}
	}
}