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
        Fog {
            Mode Off
        }
        
        pass {
		
		    Cull Front
		
		    CGPROGRAM
		    
		    #pragma vertex vert_img
		    #pragma fragment frag
		    #include "UnityCG.cginc"
            
		    float4 _ToonColor;
		    float _Brightness;
		    
		    float4 frag(v2f_img i) : COLOR {			
		    	return _ToonColor*_Brightness;
		    }
		
		    ENDCG
	    }
		
		pass {
		
		    Cull Back
		
		    CGPROGRAM
		    
		    #pragma vertex vert_img
		    #pragma fragment frag
		    #include "UnityCG.cginc"
            
		    float4 _ToonColor;
		    float _Brightness;
		    
		    float4 frag(v2f_img i) : COLOR {			
		    	return _ToonColor*_Brightness;
		    }
		
		    ENDCG
	    }
	}
}
