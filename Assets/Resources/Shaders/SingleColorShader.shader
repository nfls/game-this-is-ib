Shader "这就是IB/SingleColorShader" {

	Properties {
		_ToonColor("Toon Color", color) = (1,1,1,1)
		_Brightness("Brightness", range(0,1)) = 1
	}
	
	SubShader {
	    
        Lighting Off
		
		Pass {

			Tags {
	        	"RenderType" = "Opaque"
	    	}

		    Cull Off
			Offset -1, 0
		
		    CGPROGRAM
		    
		    #pragma vertex vert
		    #pragma fragment frag
		    #pragma multi_compile_instancing
		    #include "UnityCG.cginc"

			struct appdata {
		    	float4 vertex : POSITION;
		    	UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
		    	float4 vertex : SV_POSITION;
		    	UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			UNITY_INSTANCING_BUFFER_START(Props)
            	UNITY_DEFINE_INSTANCED_PROP(float4, _ToonColor)
            	UNITY_DEFINE_INSTANCED_PROP(float, _Brightness)
        	UNITY_INSTANCING_BUFFER_END(Props)

			v2f vert(appdata v) {
            	v2f o;

            	UNITY_SETUP_INSTANCE_ID(v);
            	UNITY_TRANSFER_INSTANCE_ID(v, o);

            	o.vertex = UnityObjectToClipPos(v.vertex);
            	return o;
        	}

			float4 frag(v2f i) : COLOR {
	        	UNITY_SETUP_INSTANCE_ID(i);
	    		return UNITY_ACCESS_INSTANCED_PROP(Props, _ToonColor) * UNITY_ACCESS_INSTANCED_PROP(Props, _Brightness);
			}
		
		    ENDCG
	    }

		Pass {
			
			Tags {
	        	"LightMode" = "ShadowCaster"
	    	}

			Cull Off
			Offset -1, 0

			CGPROGRAM

			#pragma vertex vert
 			#pragma fragment frag
 			#pragma multi_compile_shadowcaster
 			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct v2f {
         		V2F_SHADOW_CASTER;
     		};

     		v2f vert(appdata_base v) {
         		v2f o;
         		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            	return o;
    		}
 
     		float4 frag(v2f i) : COLOR {
         		SHADOW_CASTER_FRAGMENT(i)
    		}

			ENDCG
		}
	}

	Fallback "Diffuse"
}
