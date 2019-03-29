Shader "这就是IB/OutlinedTextureToonShader" {

	Properties {
		_MainTexture("Main Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", color) = (1,1,1,1)
		_OutlineWidth("Outline Width", range(0,0.1)) = 0.02
		_OutlineDistance("Outline Distance", range(0,1)) = 0.5
		_ToonLevel("Toon Level", range(0,1)) = 0.5
		_ToonSteps("Toon Steps", range(0,9)) = 3
	}
	
	SubShader {
		pass { //处理光照前的pass渲染
		
		    Tags {
		        "LightMode" = "Always"
		    }
		    
		    Cull Front
		    Lighting Off
		    ZWrite On
		    
		    CGPROGRAM
		
		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"
		    
		    float4 _OutlineColor;
		    float _OutlineWidth;
		    float _OutlineDistance;
		    
		    struct appdata {
		        float4 vertex : POSITION;
		        float3 normal : NORMAL;
		    };
		    
		    struct v2f {
		    	float4 pos : SV_POSITION;
		    };

		    v2f vert (appdata v) {
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
		
		    float4 frag(v2f i) : COLOR {
		    	return _OutlineColor;
		    }
		
		    ENDCG
		}
		
		pass { //平行光的的pass渲染
		
		    Tags {
		        "LightMode" = "ForwardBase"
		    }
		    
		    Cull Back
		    
		    CGPROGRAM
		    
		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"
            
		    float4 _LightColor0;
			sampler2D _MainTexture;
			float4 _MainTexture_ST;
		    float _ToonSteps;
		    float _ToonLevel;
            
		    struct v2f {
		    	float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
		    	float3 lightDir : TEXCOORD1;
		    	float3 viewDir : TEXCOORD2;
		    	float3 normal : TEXCOORD3;
		    };
		    
		    v2f vert (appdata_base v) {
		    	v2f o;
		    	o.pos=UnityObjectToClipPos(v.vertex);//切换到世界坐标
		    	o.normal=v.normal;
		    	o.lightDir=ObjSpaceLightDir(v.vertex);
		    	o.viewDir=ObjSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTexture);
		    	return o;
		    }
		    		
		    float4 frag(v2f i) : COLOR {
		    	float4 c=1;
		    	float3 N=normalize(i.normal);
		    	float3 viewDir=normalize(i.viewDir);
		    	float3 lightDir=normalize(i.lightDir);
		    	float diff=max(0,dot(N,i.lightDir));//求漫反射颜色
		    	diff=(diff+1)/2;//做亮化处理
		    	diff=smoothstep(0,1,diff);//使颜色平滑的在[0,1]范围之内
		    	float toon=floor(diff*_ToonSteps)/_ToonSteps;//把颜色做离散化处理，把diffuse颜色限制在_ToonSteps种
		    	diff=lerp(diff,toon,_ToonLevel);//调节比重
		    	c=tex2D(_MainTexture, i.uv)*_LightColor0*(diff);//颜色混合
		    	
		    	return c;
		    }
		    
		    ENDCG
		}
		/*
		pass { //附加点光源的pass渲染
		
		    Tags {
		        "LightMode" = "ForwardAdd"
		    }
		    
		    Blend One One
		    Cull Back
		    ZWrite Off
		    
		    CGPROGRAM
		    
		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"
            
		    float4 _LightColor0;
			sampler2D _MainTexture;
			float4 _MainTexture_ST;
		    float _ToonSteps;
		    float _ToonLevel;
            
		    struct v2f { 
		    	float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
		    	float3 lightDir:TEXCOORD1;
		    	float3 viewDir:TEXCOORD2;
		    	float3 normal:TEXCOORD3;
		    };
            
		    v2f vert (appdata_base v) {
		    	v2f o;
		    	o.pos=UnityObjectToClipPos(v.vertex);
				o.uv=TRANSFORM_TEX(v.texcoord, _MainTexture);
		    	o.normal=v.normal;
		    	o.viewDir=ObjSpaceViewDir(v.vertex);
		    	o.lightDir=_WorldSpaceLightPos0-v.vertex;
		    	return o;
		    }
		    	
		    float4 frag(v2f i) : COLOR {
		    	float4 c=1;
		    	float3 N=normalize(i.normal);
		    	float3 viewDir=normalize(i.viewDir);
		    	float dist=length(i.lightDir);//求到光源的距离
		    	float3 lightDir=normalize(i.lightDir);
		    	float diff=max(0,dot(N,i.lightDir));
		    	diff=(diff+1)/2;
		    	diff=smoothstep(0,1,diff);
		    	float atten=1/(dist);//求衰减
		    	float toon=floor(diff*atten*_ToonSteps)/_ToonSteps;
		    	diff=lerp(diff,toon,_ToonLevel);
		    	half3 h = normalize (lightDir + viewDir);//求半角向量
		    	float nh = max (0, dot (N, h));
		    	float spec = pow (nh, 32.0);//求高光强度
		    	float toonSpec=floor(spec*atten*2)/ 2;//高光离散化
		    	spec=lerp(spec,toonSpec,_ToonLevel);//调节比重
		    	c=tex2D(_MainTexture, i.uv)*_LightColor0*(diff+spec);//求最终颜色
		    	return c;
		    }
		    
		    ENDCG
		}
		*/
	}

	Fallback "Diffuse"
}
