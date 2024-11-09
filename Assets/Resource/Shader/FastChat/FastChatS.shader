Shader "CustomShader/FastChatS"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ColorTex("ColorTex",2D) = "white" {}
		_Scale("Scale",Float) = 10.0
		_Rotation("Rotation",Float) = 0.0
		_SelectedRegion("SelectedRegion", Float) = -1.0
		_RegionSize("RegionSize",Float)=9.0
		_AngleOffset("AngleOffset",Float)=6.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
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
					float4 pos : SV_POSITION;
				};

				sampler2D _MainTex;
				sampler2D _ColorTex;
				float4 _MainTex_ST;
				float _Scale;
				float _Rotation;
				float _SelectedRegion;
				float _RegionSize;
				float _AngleOffset;

				v2f vert(appdata v)
				{
					v2f o;

					float2 uvCenter = float2(0.5,0.5);
					float2 uv = v.uv - uvCenter;

					float cosR = cos(_Rotation);
					float sinR = sin(_Rotation);
					float2x2 rotationMatrix = float2x2(cosR,-sinR,sinR,cosR);

					uv = mul(rotationMatrix, uv * _Scale);
					o.uv = uv + uvCenter;

					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv = clamp(i.uv, 0.0, 1.0);
					float angle = atan2(i.uv.y - 0.5,i.uv.x - 0.5) * 180 / 3.14159;

					angle -= (45+_AngleOffset);

					if (angle < 0)angle += 360;

					float regionButton=360/_RegionSize;

					int region = int((_RegionSize - 1) - (int(round(angle) / regionButton)));

					half4 col = tex2D(_MainTex, uv);
					half4 colorOverlay = tex2D(_ColorTex,i.uv);

					if (region == int(_SelectedRegion))
					{
						colorOverlay.a = 1.0;
					}
						else 
						{
							colorOverlay.a = 0.2;
					}

					col.rgb *= colorOverlay.rgb;

					col.a *= colorOverlay.a;

					return col;
				}
				ENDCG
			}
		}
}
