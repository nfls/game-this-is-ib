Shader "WilliamXie/UnlitOutlinedShader"
{
	Properties {
		_MainColor("Main Color",color)=(1,1,1,1)
		_Brightness("Brightness",range(0,1))=1
		_OutlineColor("Outline Color",color)=(1,1,1,1)
		_OutlineWidth("Outline Width",range(0,0.1))=0.02
		_OutlineDistance("Outline Distance",range(0,1))=0.5
	}
	SubShader {
		pass{
		Cull Front
		ZWrite On
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		
		float4 _OutlineColor;
		float _OutlineWidth;
		float _OutlineDistance;
		
		struct v2f {
			float4 pos:SV_POSITION;
		};

		v2f vert (appdata_full v) {
			v2f o;
			float3 dir=normalize(v.vertex.xyz);
			float3 dir2=v.normal;
			float D=dot(dir,dir2);
			dir=dir*sign(D);
			dir=dir*_OutlineDistance+dir2*(1-_OutlineDistance);
			v.vertex.xyz+=dir*_OutlineWidth;
			o.pos=UnityObjectToClipPos(v.vertex);
			return o;
		}
		float4 frag(v2f i):COLOR
		{
			float4 c=_OutlineColor;
			return c;
		}
		ENDCG
		}
		pass{
		Cull Back
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		float4 _MainColor;
		float _Brightness;
		
		struct v2f {
		    float4 pos:SV_POSITION;
		};
		
		v2f vert (appdata_base v) {
		    v2f o;
		    o.pos=UnityObjectToClipPos(v.vertex);
		    return o;
		}
			
		float4 frag(v2f i):COLOR
		{			
			return _MainColor*_Brightness;
		}
		ENDCG
		}
	}
}
