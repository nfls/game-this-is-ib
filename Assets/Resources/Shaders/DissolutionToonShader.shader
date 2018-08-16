Shader "这就是IB/DissolutionShader" {

	Properties {
	    _ToonColor("Toon Color", color) = (1,1,1,1)
	    _ToonLevel("Toon Level", range(0,1)) = 0.5
		_ToonSteps("Toon Steps", range(0,9)) = 3
		_DissolveColor("Dissolve Color", Color) = (0,0,0,0)
		_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,1)
		_DissolveMap("Dissolve Map", 2D) = "white"{}
		_DissolveThreshold("Dissolve Threshold", Range(0,1)) = 0
		_ColorFactor("Color Factor", Range(0,1)) = 0.7
		_DissolveEdge("Dissolve Edge", Range(0,1)) = 0.8
		_FlyThreshold("Fly Threshold", Range(0,1)) = 0.7
		_FlyFactor("Fly Factor", Range(0,1)) = 0.7
	}
	
	SubShader
	{
		Tags {
		    "RenderType" = "Opaque"
		    "LightMode" = "ForwardBase"
		}
		
		Pass {
		
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
	    
	        float4 _LightColor0;
            float4 _ToonColor;
            float _ToonSteps;
            float _ToonLevel;
	        fixed4 _DissolveColor;
	        fixed4 _DissolveEdgeColor;
	        sampler2D _DissolveMap;
	        float _DissolveThreshold;
	        float _ColorFactor;
	        float _DissolveEdge;
	        float _FlyThreshold;
	        float _FlyFactor;
	        
	        struct v2f {
	        	float4 pos : SV_POSITION;
	        	float3 worldNormal : TEXCOORD0;
	        	float2 uv : TEXCOORD1;
	        	float3 lightDir : TEXCOORD2;
	        	float3 viewDir : TEXCOORD3;
	        	float3 normal : TEXCOORD4;
	        };
	        
	        v2f vert(appdata_base v) {
	        	v2f o;
	        	// v.vertex.xyz+=v.normal * _DissolveThreshold * 0.5;
	        	v.vertex.xyz += v.normal * saturate(_DissolveThreshold - _FlyThreshold) * _FlyFactor;
	        	o.pos = UnityObjectToClipPos(v.vertex);
	        	o.uv = v.texcoord;
	        	o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
	        	o.normal=v.normal;
	        	o.lightDir=ObjSpaceLightDir(v.vertex);
	        	o.viewDir=ObjSpaceViewDir(v.vertex);
	        	
	        	return o;
	        }
	        
	        fixed4 frag(v2f i) : SV_Target {
	        	//采样Dissolve Map
	        	fixed4 dissolveValue = tex2D(_DissolveMap, i.uv);
	        	//小于阈值的部分直接discard
	        	if (dissolveValue.r < _DissolveThreshold) {
	        		discard;
	        	}
	            
	            float4 c=1;
	        	float3 N=normalize(i.normal);
	        	float3 viewDir=normalize(i.viewDir);
	        	float3 lightDir=normalize(i.lightDir);
	        	float diff=max(0,dot(N,i.lightDir));
	        	diff=(diff+1)/2;
	        	diff=smoothstep(0,1,diff);
	        	float toon=floor(diff*_ToonSteps)/_ToonSteps;
	        	diff=lerp(diff,toon,_ToonLevel);
	        	c=_ToonColor*_LightColor0*(diff);
            
	        	float percentage = _DissolveThreshold / dissolveValue.r;
	        	//如果当前百分比 - 颜色权重 - 边缘颜色u
	        	float lerpEdge = sign(percentage - _ColorFactor - _DissolveEdge);
	        	//貌似sign返回的值还得saturate一下，否则是一个很奇怪的值
	        	fixed3 edgeColor = lerp(_DissolveEdgeColor.rgb, _DissolveColor.rgb, saturate(lerpEdge));
	        	//最终输出颜色的lerp值
	        	float lerpOut = sign(percentage - _ColorFactor);
	        	//最终颜色在原颜色和上一步计算的颜色之间差值（其实经过saturate（sign（..））的lerpOut应该只能是0或1）
	        	fixed3 colorOut = lerp(c, edgeColor, saturate(lerpOut));
	        	return fixed4(colorOut, 1);
	        }
	        ENDCG
		}
	}
	
	FallBack "Diffuse"
}

