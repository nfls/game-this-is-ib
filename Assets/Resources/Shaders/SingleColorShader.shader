Shader "这就是IB/SingleColorShader" {

	Properties {
		_ToonColor("Toon Color", color) = (1,1,1,1)
		_Brightness("Brightness", range(0,1)) = 1
	}
	
	SubShader {
	
	    Tags {
	        "RenderType" = "Opaque"
	    }
	    
        Lighting Off
		
		Pass {
		
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
/*
		Pass {
			
			Cull Front

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
*/
	}
}
