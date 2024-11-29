Shader "Custom/LocalGlowEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ExplosionCenter ("Explosion Center", Vector) = (0, 0, 0, 0)
        _GlowRadius ("Glow Radius", Float) = 5.0
        _GlowIntensity ("Glow Intensity", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "AlphaTest" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _ExplosionCenter;
            float _GlowRadius;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_MainTex, i.uv);
                float explosion_distance = distance(i.worldPos, _ExplosionCenter.xyz);
                float glowFactor = saturate(1.0 - (explosion_distance / _GlowRadius));
                col.rgb += glowFactor * _GlowIntensity;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
