// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "myshader/ImageA"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_AlphaLX("RangeAlphaLX",Float) = 0
		_AlphaRX("RangeAlphaRX",Float) = 1
		_AlphaTY("RangeAlphaTY",Float) = 1
		_AlphaBY("RangeAlphaBY",Float) = 0
		_AlphaPower("Power",Float) = 0
	}
		SubShader
		{
			Tags{ "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			//Cull Back

				Cull Off
				Lighting Off
				ZWrite Off
				Fog{ Mode Off }

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
				float4 _MainTex_ST;
				float _AlphaPower;
				sampler2D _AlphaTex;
				float _AlphaLX;
				float _AlphaRX;
				float _AlphaTY;
				float _AlphaBY;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}
				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

			#if ETC1_EXTERNAL_ALPHA
					// get the color from an external texture (usecase: Alpha support for ETC1 on android)
					color.a = tex2D(_AlphaTex, uv).r;
			#endif //ETC1_EXTERNAL_ALPHA

					return color;
				}
				//step（a,b)既是当b>=a时返回1，否则返回0
				//lerp(a, b, w) 根据w返回a到b之间的插值 a, b, w相当于 float3 lerp(float3 a, float3 b, float w) { return a + w * (b - a); } 由此可见 当 w = 0时返回a.当w = 1时 返回b
				//saturate函数（saturate(x)的作用是如果x取值小于0，则返回值为0。如果x取值大于1，则返回值为1。若x在0到1之间，则直接返回x的值.）
				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = SampleSpriteTexture(i.uv);
					fixed alphalx = col.a * lerp(1,_AlphaPower,(_AlphaLX - (1 - i.uv.y + i.uv.x) / 2));
					col.a = saturate(lerp(alphalx,col.a,step(_AlphaLX, (1 - i.uv.y + i.uv.x) / 2)));

					fixed alpharx = col.a * lerp(1,_AlphaPower,((1 - i.uv.y + i.uv.x) / 2 - _AlphaRX));
					col.a = saturate(lerp(col.a,alpharx,step(_AlphaRX, (1 - i.uv.y + i.uv.x) / 2)));

					fixed alphaby = col.a * lerp(1,_AlphaPower,(_AlphaBY - (i.uv.y + i.uv.x) / 2));
					col.a = saturate(lerp(alphaby,col.a,step(_AlphaBY, (i.uv.y + i.uv.x) / 2)));

					fixed alphaty = col.a * lerp(1,_AlphaPower,((i.uv.y + i.uv.x) / 2 - _AlphaTY));
					col.a = saturate(lerp(col.a,alphaty,step(_AlphaTY, (i.uv.y + i.uv.x) / 2)));


					return col;
				}
				ENDCG
			}
		}
}
