Shader "CustomShader/GallerySpriteOutlineWithShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _OutlineAlpha("OutlineAlpha",Range(0,1))=1
        _OutlinePixelWidth ("OutlinePixelWidth", Range(0,10)) = 1
        _Alpha("Alpha",Range(0,1))=0.2
    }

    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
        LOD 200
        // Pass for outline
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 _OutlineColor;
            float _OutlineAlpha;
            float _Alpha;
            int _OutlinePixelWidth;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float2 uvOutDistTex : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                //------Outline------
                fixed4 col = tex2D(_MainTex, i.uv);
                float originalAlpha = col.a;

                float2 destUv = float2(_OutlinePixelWidth * _MainTex_TexelSize.x, _OutlinePixelWidth * _MainTex_TexelSize.y);


                float spriteLeft = tex2D(_MainTex, i.uv + float2(destUv.x, 0)).a;
                float spriteRight = tex2D(_MainTex, i.uv - float2(destUv.x, 0)).a;
                float spriteBottom = tex2D(_MainTex, i.uv + float2(0, destUv.y)).a;
                float spriteTop = tex2D(_MainTex, i.uv - float2(0, destUv.y)).a;
                float spriteTopLeft = tex2D(_MainTex, i.uv + float2(destUv.x, destUv.y)).a;
                float spriteTopRight = tex2D(_MainTex, i.uv + float2(-destUv.x, destUv.y)).a;
                float spriteBotLeft = tex2D(_MainTex, i.uv + float2(destUv.x, -destUv.y)).a;
                float spriteBotRight = tex2D(_MainTex, i.uv + float2(-destUv.x, -destUv.y)).a;
                float result = spriteLeft + spriteRight + spriteBottom + spriteTop + spriteTopLeft + spriteTopRight + spriteBotLeft + spriteBotRight;

                result = step(0.05, saturate(result));
                result *= (1 - originalAlpha) * _OutlineAlpha;

                fixed4 outline = _OutlineColor;
                col = lerp(col, outline, result);

                return col*_Alpha;
            }

            ENDCG
        }
    }
}
