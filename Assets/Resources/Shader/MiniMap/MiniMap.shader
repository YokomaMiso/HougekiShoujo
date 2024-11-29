Shader "CustomShader/MiniMapWithOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PlayerSize("PlayerSize",Range(0.001,0.01))=0.01
        _MaxPlayer("MaxPlayer",Float)=6
        _TeamAColor("TeamAColor",Color)=(0.1255, 0.3137, 0.8941, 1)
        _TeamBColor("TeamBColor",Color)=(1, 0, 0, 1)

    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
{
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
    };

    sampler2D _MainTex;

    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
    }

    fixed4 frag (v2f i) : SV_Target
    {
        return tex2D(_MainTex, i.uv);
    }
    ENDCG
}

        // プレーヤー描画
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragPlayerPass

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
            };

            float3 _PlayerPositions[6];
            float _PlayerTeam[6];
            float _PlayerSize;
            float _MaxPlayer;
            float4 _TeamAColor;
            float4 _TeamBColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 fragPlayerPass(v2f i) : SV_Target
            {
                fixed4 playerColor = fixed4(0, 0, 0, 0); 


                for (int idx = 0; idx < _MaxPlayer; idx++)
                {
                    float3 playerPos = _PlayerPositions[idx];

                    
                    float2 playerUV = playerPos.xz;
                    float distance = length(playerUV - i.uv);

                    
                    if (distance < _PlayerSize)
                    {
                        if (_PlayerTeam[idx] == 0) 
                            playerColor = _TeamAColor;
                        else 
                            playerColor = _TeamBColor;
                    }
                }

                return playerColor;
            }
            ENDCG
        }
    }
}
