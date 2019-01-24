Shader "这就是IB/WaterShader" {
    Properties{
        _MainTint("Main Tint", Color) = (1,1,1,0)
        _BackTint("Back Tint", Color) = (1,1,1,0)
        _NormalTex("Normal Map", 2D) = "bump" { }
        _TransVal("Transparecy Value", Range(0, 1)) = 0.5
        _Tess("Tessellation Level", Range(1, 32)) = 4
        _Phong("Phong Level", Range(0, 1)) = 0.5
    }  
    SubShader{
         Tags { "Queue" = "Transparent" "RenderType" = "Opaque" } // 用来保证渲染顺序在透明之前
         CGPROGRAM
            #include "UnityCG.cginc"
            #pragma surface surf None vertex:vert tessphong:_Phong alpha

            float4 _MainTint;
            float4 _BackTint;
            sampler2D _NormalTex;
            float _TransVal;
            
            struct Input {
                float2 uv_NormalTex;
                float4 worldPos;
                INTERNAL_DATA
            };

            inline fixed4 LightingNone (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten){
                return float4(s.Albedo, s.Alpha);
            }
            
            void vert (inout appdata_full v) {
                // UNITY_INITIALIZE_OUTPUT(Input, o);
                // o.pos=v.vertex;
            }
            
            void surf (Input IN, inout SurfaceOutput o){
                float3 normalMap = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
                float3 finalColor = _MainTint.rgb * _BackTint;
                o.Normal = normalize(normalMap.rgb + o.Normal.rgb);
                o.Gloss = 1.0;
                o.Albedo = finalColor;
                o.Alpha = (_MainTint.a * 0.5+0.5) * _TransVal;
            }
         ENDCG
    }


    FallBack "Diffuse"
}