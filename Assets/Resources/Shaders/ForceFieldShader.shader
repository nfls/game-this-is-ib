Shader "这就是IB/ForceFieldShader" {

	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_DistortionPower ("Distortion Power", Float) = 1
		_RimlightPower ("Rimlight Power", Range (1, 10)) = 3
		_IntersectionPower ("Intersection Power", Float) = 2
		_TimeFactor ("Time Factor", Float) = -30
		_DistanceFactor ("Distance Factor", Float) = 60
		_TotalFactor ("Total Factor", Float) = 1
		_WaveWidth ("Wave Width", Range (0, 2)) = .3
		_MaxWaveDistance ("Maximum Wave Distance", Float) = 1
	}

	SubShader {

		Cull Off ZWrite Off ZTest On Blend SrcAlpha OneMinusSrcAlpha

		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		GrabPass {
			"_GrabTex"
		}

		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 pos : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 grabPos : TEXCOORD3;
			};

			sampler2D _GrabTex;
			float4 _GrabTex_ST;
			fixed4 _Color;
			float _DistortionPower;
			float _RimlightPower;
			float _IntersectionPower;
			half4x4 _Hits;
			float _TimeFactor;
			float _DistanceFactor;
			float _TotalFactor;
			float _WaveWidth;
			float _MaxWaveDistance;
			float4 _CurrentWaveDists;
			sampler2D _CameraDepthTexture;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.pos = v.vertex;
				o.viewDir = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex));
				o.screenPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.screenPos.z);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
				float screenZ = i.screenPos.z;

				float intersection = 1 - min(abs(sceneZ - screenZ) / _IntersectionPower, 1);

				// float intersection = (1 - (sceneZ - screenZ)) * _IntersectionPower;
				float rimlight = 1 - abs(dot(normalize(i.normal), normalize(i.viewDir))) * _RimlightPower;
				float glow = max(intersection, rimlight);

				float timeFactor = -_Time.y * _TimeFactor;
				float totalFactor = _TotalFactor * .01 / _WaveWidth;

				float3 dv0 = _Hits[0].xyz - i.pos;
				float dist0 = sqrt(dv0.x * dv0.x + dv0.y * dv0.y + dv0.z * dv0.z);
				dv0 = normalize(dv0);
				float sinFactor0 = sin(dist0 * _DistanceFactor + timeFactor);
				float discardFactor0 = clamp(_WaveWidth - abs(_CurrentWaveDists[0] - dist0), 0, 1);
				float2 offset0 = dv0 * sinFactor0 * discardFactor0 * clamp((1 - dist0 / _MaxWaveDistance), 0, 1);

				float3 dv1 = _Hits[1].xyz - i.pos;
				float dist1 = sqrt(dv1.x * dv1.x + dv1.y * dv1.y + dv1.z * dv1.z);
				dv1 = normalize(dv1);
				float sinFactor1 = sin(dist1 * _DistanceFactor + timeFactor);
				float discardFactor1 = clamp(_WaveWidth - abs(_CurrentWaveDists[1] - dist1), 0, 1);
				float2 offset1 = dv1 * sinFactor1 * discardFactor1 * clamp((1 - dist1 / _MaxWaveDistance), 0, 1);

				float3 dv2 = _Hits[2].xyz - i.pos;
				float dist2 = sqrt(dv2.x * dv2.x + dv2.y * dv2.y + dv2.z * dv2.z);
				dv2 = normalize(dv2);
				float sinFactor2 = sin(dist2 * _DistanceFactor + timeFactor);
				float discardFactor2 = clamp(_WaveWidth - abs(_CurrentWaveDists[2] - dist2), 0, 1);
				float2 offset2 = dv2 * sinFactor2 * discardFactor2 * clamp((1 - dist2 / _MaxWaveDistance), 0, 1);

				float3 dv3 = _Hits[3].xyz - i.pos;
				float dist3 = sqrt(dv3.x * dv3.x + dv3.y * dv3.y + dv3.z * dv3.z);
				dv3 = normalize(dv3);
				float sinFactor3 = sin(dist3 * _DistanceFactor + timeFactor);
				float discardFactor3 = clamp(_WaveWidth - abs(_CurrentWaveDists[3] - dist3), 0, 1);
				float2 offset3 = dv3 * sinFactor3 * discardFactor3 * clamp((1 - dist3 / _MaxWaveDistance), 0, 1);

				float2 offset = (offset0 + offset1 + offset2 + offset3);
				i.grabPos.xy += offset * _DistortionPower;
				
				fixed4 finalColor = tex2Dproj(_GrabTex, i.grabPos);
				finalColor += _Color * glow;
				/*
				fixed a = 0;
				if (abs(sceneZ - screenZ) / _IntersectionPower > 1) a = 1;
				return fixed4(a, 0, 0, 1);
				*/
				return finalColor;
			}

			ENDCG
		}
	}
}
