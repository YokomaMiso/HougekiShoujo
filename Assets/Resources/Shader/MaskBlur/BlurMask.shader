Shader "Custom/ScrollViewGradientMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ViewHeight ("View Height", Float) = 500
        _FadeStart ("Fade Start", Float) = 100
        _FadeEnd ("Fade End", Float) = 400
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _ViewHeight;
            float _FadeStart; 
            float _FadeEnd; 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float relativeY : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.relativeY = v.vertex.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float alpha = 1.0;
                if (i.relativeY < _FadeStart)
                {
                    alpha = saturate((i.relativeY - _FadeStart + _ViewHeight) / _FadeStart);
                }
                else if (i.relativeY > _FadeEnd)
                {
                    alpha = saturate((_ViewHeight - i.relativeY) / (_ViewHeight - _FadeEnd));
                }

                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
