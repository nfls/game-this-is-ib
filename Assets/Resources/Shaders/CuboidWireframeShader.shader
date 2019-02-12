Shader "这就是IB/CuboidWireframeShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Thickness("Thickness", Range (0, 10)) = 3
        _ToonColor("Toon Color", color) = (1,1,1,1)
		_ToonLevel("Toon Level", range(0,1)) = 0.5
		_ToonSteps("Toon Steps", range(0,9)) = 3
    }
    
    SubShader
    {
        Pass
        {
            Tags {
                "RenderType"="Opaque"
                "Queue"="Geometry"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
            CGPROGRAM

            #include "ThisIsIB.cginc"
            #pragma target 4.0
            #pragma vertex vert_wireframe
            #pragma geometry geom_wireframe
            #pragma fragment frag_wireframe
            ENDCG
        }
    }
}