Shader "这就是IB/ForceShieldShader" {

	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_DistortionPower ("Distortion Power", Float) = 1
		_RimlightColor ("Rimlight Color", Color) = (1, 1, 1, 1)
		_RimlightPower ("Rimlight Power", Range (1, 10)) = 3
		_IntersectionPower ("Intersection Power", Range (0, 3)) = .5
		_MaxDistance ("Maximum Distance", Float) = 1
	}

	SubShader {

		Cull Off ZWrite Off ZTest On Blend SrcAlpha OneMinusSrcAlpha

		Pass {

			Tags {
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 pos : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _DistortionPower;
			fixed4 _RimlightColor;
			fixed _RimlightPower;
			fixed _IntersectionPower;
			float4x4 _Hits;
			float4 _HitAreaAlphas;
			float _MaxDistance;
			sampler2D _CameraDepthTexture;

			v2f vert (appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.pos = v.vertex;
				o.viewDir = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex));
				o.screenPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.screenPos.z);
				return o;
			}

			fixed4 frag (v2f i, float face : VFACE) : SV_Target {
				fixed sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
				fixed screenZ = i.screenPos.z;

				fixed intersection = 1 - min(abs(sceneZ - screenZ) / _IntersectionPower, 1);

				fixed rimlight = 1 - abs(dot(normalize(i.normal), normalize(i.viewDir))) * _RimlightPower;
				fixed glow = max(intersection, rimlight);

				float3 dv0 = _Hits[0].xyz - i.pos;
				float dist0 = sqrt(dv0.x * dv0.x + dv0.y * dv0.y + dv0.z * dv0.z);
				float d0 = dist0 / _MaxDistance;
				float factor0 = step(dist0, _MaxDistance) * step(1 - _HitAreaAlphas[0], d0) * (1 - d0);

				float3 dv1 = _Hits[1].xyz - i.pos;
				float dist1 = sqrt(dv1.x * dv1.x + dv1.y * dv1.y + dv1.z * dv1.z);
				float d1 = dist1 / _MaxDistance;
				float factor1 = step(dist1, _MaxDistance) * step(1 - _HitAreaAlphas[1], d1) * (1 - d1);

				float3 dv2 = _Hits[2].xyz - i.pos;
				float dist2 = sqrt(dv2.x * dv2.x + dv2.y * dv2.y + dv2.z * dv2.z);
				float d2 = dist2 / _MaxDistance;
				float factor2 = step(dist2, _MaxDistance) * step(1 - _HitAreaAlphas[2], d2) * (1 - d2);

				float3 dv3 = _Hits[3].xyz - i.pos;
				float dist3 = sqrt(dv3.x * dv3.x + dv3.y * dv3.y + dv3.z * dv3.z);
				float d3 = dist3 / _MaxDistance;
				float factor3 = step(dist3, _MaxDistance) * step(1 - _HitAreaAlphas[3], d3) * (1 - d3);

				float factor = max(max(max(factor0, factor1), factor2), factor3);
				
				fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
				fixed4 finalColor;
				finalColor.a = factor * 3 * texColor.a;
				finalColor.rgb = texColor.rgb * step(.01, finalColor.a);
				finalColor += _RimlightColor * glow;

				return finalColor;
			}

			ENDCG
		}
	}
}
