Shader "这就是IB/SceneScanEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" { }
		_Distance ("Distance", Float) = 0
		_Width ("Width", Float) = 10
		_Color ("Color", Color) = (1, 1, 1, 0)
		_NoiseTex ("Noise Texture", 3D) = "" {}
		_NoisePower ("Noise Power", Float) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler3D _NoiseTex;
			sampler2D _CameraDepthTexture;
			float4x4 _FrustumCornersWS;
			fixed4 _Color;
			float _Distance;
			float _Width;
			float3 _CenterWorldSpacePos;
			float _NoisePower;
			
			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				int xx = (int) v.vertex.x;
				int yy = (int) v.vertex.y;
				int z = abs (3 - xx - 3 * yy);

				o.ray = _FrustumCornersWS[z];
				o.ray.w = v.vertex.z;

				return o;
			}
			
			/*fixed4 frag (v2f i) : SV_Target
			{
				float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv)));
				float3 worldPos = (depth * i.ray).xyz + _WorldSpaceCameraPos;
				float3 ds = worldPos - _CenterWorldSpacePos;
				float distance = sqrt(ds.x * ds.x + ds.y * ds.y + ds.z * ds.z);
				float noise = tex3D(_NoiseTex, worldPos).r * _NoisePower;
				// return noise;
				float currentDistance = _Distance + noise - _NoisePower / 2;
				float minDistance = _Distance - _Width;
				float coefficient = (distance - minDistance) / (currentDistance - minDistance) * step(distance, currentDistance);

				fixed4 color = tex2D(_MainTex, i.uv);
				// return coefficient > 0 ? color + _Color * coefficient : color;
				fixed flag = step(.001, coefficient);
				// return (color + _Color * coefficient) * flag + color * (1 - flag);
				return lerp(color, _Color, coefficient) * flag + color * (1 - flag);
			}*/

			fixed4 frag (v2f i) : SV_Target
			{
				float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv)));
				float3 worldPos = (depth * i.ray).xyz + _WorldSpaceCameraPos;
				float3 ds = worldPos - _CenterWorldSpacePos;
				float distance = sqrt(ds.x * ds.x + ds.y * ds.y + ds.z * ds.z);
				float noise = tex3D(_NoiseTex, worldPos).r * _NoisePower;
				float currentDistance = _Distance + noise - _NoisePower / 2;
				fixed4 color = tex2D(_MainTex, i.uv);
				fixed flag = step(distance, currentDistance);
				fixed c = (color.r + color.g + color.b) / 3;
				// return color * flag + fixed4(c, c, c, 1) * (1 - flag);
				// return color * flag + (1 - color) * (1 - flag);
				return color * flag + _Color * (1 - flag);
			}

			ENDCG
		}
	}
}