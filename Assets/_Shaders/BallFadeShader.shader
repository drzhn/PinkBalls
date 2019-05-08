Shader "Custom/BallFadeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        // Y-координата камеры всегда _MaxHeight/2 и Camera.orthographicSize = _MaxHeight/2
        // то есть по ходу движения шарики меняют свою Y координату с 0 до _MaxHeight, 
        // что влияет на Alpha их цвета
        _MaxHeight("_Maxheight", Float) = 10 
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex; // на всякий случай оставлю
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _MaxHeight;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a * clamp(1-IN.worldPos.y / _MaxHeight, 0,1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
