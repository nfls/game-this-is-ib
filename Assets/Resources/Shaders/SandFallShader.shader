Shader "这就是IB/SandFallShader"
{
    Properties
        {
            _MainTex ("Texture", 2D) = "white" {}
            _Level("Level", Range (0, 1)) = 0
            _Speed("Speed", Float) = -3
            _Acceleration("Acceleration", Float) = 10
            _FloorY("Floor Y",Float) = -0.5
            _FallSpeed("Fall Speed",Float) = 0.4
        }
        SubShader
        {
            Tags { "RenderType"="Opaque" }
            
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma geometry geom
    
                #include "UnityCG.cginc"
    
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
    
                struct v2g
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : POSITION;
                };
    
                struct g2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
    
                sampler2D _MainTex;
    
                float _Speed;
                float _Level;
                float _Acceleration;
                float _StartTime;
                float _FloorY;
                float _FallSpeed;
    
                v2g vert (appdata v)
                {
                    v2g o;
                    o.vertex = v.vertex;
                    o.uv = v.uv;
                    return o;
                }
    
                [maxvertexcount(1)]
                void geom(triangle v2g IN[3], inout PointStream<g2f> pointStream)
                {
                    g2f o;
    
                    float3 v1 = IN[1].vertex - IN[0].vertex;
                    float3 v2 = IN[2].vertex - IN[0].vertex;
    
                    float3 norm = normalize(cross(v1, v2));
    
                    float3 tempPos = (IN[0].vertex + IN[1].vertex + IN[2].vertex) / 3;
    
                    // float realTime = _Time.y - _StartTime;
    
    
                    // 修改
                    float3 worldPos = mul(unity_ObjectToWorld, tempPos).xyz;            // 获取顶点的世界坐标
                    // worldPos.y -= _Speed * realTime + .5 * _Acceleration * pow(realTime, 2);
                    worldPos.y -= _Speed * _Level + .5 * _Acceleration * pow(_Level, 2);
    
                    if(_FloorY<worldPos.y)
                    {
                        // tempPos -= norm * (_FallSpeed * realTime);
                        tempPos -= norm * (_FallSpeed * _Level);
                        worldPos = mul(unity_ObjectToWorld, tempPos).xyz;           // 再算一次顶点的世界坐标
                        // worldPos.y -= _Speed * realTime + .5 * _Acceleration * pow(realTime, 2);
                        worldPos.y -= _Speed * _Level + .5 * _Acceleration * pow(_Level, 2);
                    }
                    else
                    {
                        worldPos.y = max(_FloorY, worldPos.y);
                    }
    
                    tempPos = mul(unity_WorldToObject, worldPos).xyz;
    
                    o.vertex = UnityObjectToClipPos(tempPos);
    
                    o.uv = (IN[0].uv + IN[1].uv + IN[2].uv) / 3;
    
                    pointStream.Append(o);
                }
    
                fixed4 frag (g2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    return col;
                }
                ENDCG
            }
        }
}