#ifndef THIS_IS_IB_CG_INCLUDE
#define THIS_IS_IB_CG_INCLUDE

    #include "UnityCG.cginc"

    float _Thickness;
    fixed4 _Color;
    fixed4 _LightColor0;
	fixed4 _ToonColor;
	float _ToonSteps;
	float _ToonLevel;
    float4 _MainTex_ST;
    sampler2D _MainTex;
    
    struct v2g_wireframe {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
        float3 lightDir : TEXCOORD1;
		float3 normal : TEXCOORD2;
    };
    
    struct g2f_wireframe {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
        float3 dist : TEXCOORD1;
        float3 lightDir : TEXCOORD2;
		float3 normal : TEXCOORD3;
    };
    
    v2g_wireframe vert_wireframe(appdata_base v) {
        v2g_wireframe o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.lightDir = ObjSpaceLightDir(v.vertex);
        o.normal = v.normal;
        return o;
    }
    
    [maxvertexcount(3)]
    void geom_wireframe(triangle v2g_wireframe p[3], inout TriangleStream<g2f_wireframe> triStream) {
        v2g_wireframe np[3];
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
        
        g2f_wireframe pIn;
        
        pIn.lightDir = np[0].lightDir;
        pIn.normal = np[0].normal;
        pIn.pos = np[0].pos;
        pIn.uv = np[0].uv;
        pIn.dist = float3(dist0, 0, 0);
        triStream.Append(pIn);
        
        pIn.lightDir = np[1].lightDir;
        pIn.normal = np[1].normal;
        pIn.pos = np[1].pos;
        pIn.uv = np[1].uv;
        pIn.dist = float3(0, dist1, 0);
        triStream.Append(pIn);
        
        pIn.lightDir = np[2].lightDir;
        pIn.normal = np[2].normal;
        pIn.pos = np[2].pos;
        pIn.uv = np[2].uv;
        pIn.dist = float3(0, 0, dist2);
        triStream.Append(pIn);
    }

    inline fixed4 getcolor(float4 pos, float2 uv, float3 dist, float3 lightDir, float3 normal) {
		fixed3 N=normalize(normal);
		fixed3 lDir=normalize(lightDir);
		fixed diff=max(0,dot(N,lDir));//求漫反射颜色
		diff=(diff+1)/2;//做亮化处理
		diff=smoothstep(0,1,diff);//使颜色平滑的在[0,1]范围之内
		fixed toon=floor(diff*_ToonSteps)/_ToonSteps;//把颜色做离散化处理，把diffuse颜色限制在_ToonSteps种
		diff=lerp(diff,toon,_ToonLevel);//调节比重
		fixed3 transColor=_ToonColor*tex2D(_MainTex, uv)*_LightColor0*(diff);//颜色混合
		
        fixed4 color = (_Thickness > dist.y || _Thickness  > dist.z) ? _Color : fixed4(transColor, _ToonColor.a);
        return color;
    }
    
    fixed4 frag_wireframe(g2f_wireframe input) : SV_TARGET {
        return getcolor(input.pos, input.uv, input.dist, input.lightDir, input.normal);
    }

#endif