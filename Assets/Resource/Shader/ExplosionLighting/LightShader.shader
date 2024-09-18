Shader "CustomShader/LightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpecPower("SpecPower",Range(1,128))=1
        _SpecIntensity("SpecIntensity",Color)=(1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                half3 n:NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 posWorld:TEXCOORD0;
                half3 n:NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SpecIntensity;
            float _SpecPower;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.n=UnityObjectToWorldNormal(v.n);
                o.posWorld=mul(unity_ObjectToWorld,v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = unity_AmbientSky;

                float3 n=normalize(i.n);
                float3 l=_WorldSpaceLightPos0;
                float nl=max(0,dot(n,l));
                col+=nl*_LightColor0;

                float3 r=reflect(-1,n);
                float3 e=normalize(_WorldSpaceCameraPos-i.posWorld);
                float fSpec=pow(max(0,dot(r,e)),_SpecPower);
                col+=_SpecIntensity*fSpec;


                return col;
            }
            ENDCG
        }
    }
}
