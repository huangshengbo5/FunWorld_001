// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "myshader/gaosi3"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
	}

		SubShader
	{
		LOD 200

		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}

		Pass
	{
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		//	Offset - 1, -1
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag			
#include "UnityCG.cginc"

		sampler2D _MainTex;
	float4 _MainTex_ST;

	struct appdata_t
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
	};

	v2f o;

	v2f vert(appdata_t v)
	{
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = v.texcoord;
		o.color = v.color;
		return o;
	}
	half4 _MainTex_TexelSize;
	fixed4 GetBlurColor(v2f IN)
	{

		//float space = 1.0 / _TextureSize; //算出一个像素的空间
		float space = _MainTex_TexelSize.x;
		float _BlurRadius = 2;
		int count = _BlurRadius * 2 + 1; //取值范围
		count *= count;

		//将以自己为中心，周围半径的所有颜色相加，然后除以总数，求得平均值
		fixed4 computedColor = tex2D(_MainTex, IN.texcoord) * IN.color;

		for (int x = -_BlurRadius; x <= _BlurRadius; x++)
		{
			for (int y = -_BlurRadius; y <= _BlurRadius; y++)
			{
				computedColor += tex2D(_MainTex, half2(IN.texcoord.x + x*space, IN.texcoord.y + y*space)) * IN.color;
			}
		}
		return computedColor / count;
	}


	float GetGaussianDistribution(float x, float y, float rho) {
		float g = 1.0f / sqrt(2.0f * 3.141592654f * rho * rho);
		return g * exp(-(x * x + y * y) / (2 * rho * rho));
	}
	float4 GetGaussBlurColor(v2f IN)
	{
		//算出一个像素的空间
		//float space = 1.0 / _TextureSize;
		float space = _MainTex_TexelSize.x;
		int _BlurRadius = 15;
		//参考正态分布曲线图，可以知道 3σ 距离以外的点，权重已经微不足道了。
		//反推即可知道当模糊半径为r时，取σ为 r/3 是一个比较合适的取值。
		float rho = (float)_BlurRadius * space / 3.0;

		//---权重总和
		float weightTotal = 0;
		for (int x = -_BlurRadius; x <= _BlurRadius; x++)
		{
			for (int y = -_BlurRadius; y <= _BlurRadius; y++)
			{
				weightTotal += GetGaussianDistribution(x * space, y * space, rho);
			}
		}
		//--------
		fixed4 computedColor = float4(0, 0, 0, 0);;

		for (int x = -_BlurRadius; x <= _BlurRadius; x++)
		{
			for (int y = -_BlurRadius; y <= _BlurRadius; y++)
			{
				float weight = GetGaussianDistribution(x * space, y * space, rho) / weightTotal;

				fixed4 color = tex2D(_MainTex, half2(IN.texcoord.x + x*space, IN.texcoord.y + y*space))* IN.color;
				color = color * weight;
				computedColor += color;
			}
		}
		//computedColor.rgb = float3(1.0, 0.0, 0.0);
		return computedColor;
	}
	fixed4 frag(v2f IN) : COLOR
	{
		//half4 col = IN.color;
		//col.a *= tex2D(_MainTex, IN.texcoord).a;
		//col.rgb = float3(1.0, 0.0, 0.0);
		//col = tex2D(_MainTex, IN.texcoord);
		//return col;
		//return tex2D(_MainTex, IN.texcoord) * IN.color;
		//return GetBlurColor(IN);

		return GetGaussBlurColor(IN);

		float distance = 0.01;
		fixed4 computedColor = tex2D(_MainTex, IN.texcoord) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x + distance, IN.texcoord.y + distance)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x + distance, IN.texcoord.y)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x, IN.texcoord.y + distance)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x - distance, IN.texcoord.y - distance)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x + distance, IN.texcoord.y - distance)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x - distance, IN.texcoord.y + distance)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x - distance, IN.texcoord.y)) * IN.color;
		computedColor += tex2D(_MainTex, half2(IN.texcoord.x, IN.texcoord.y - distance)) * IN.color;
		computedColor = computedColor / 9;
		return computedColor;
	}
		ENDCG
	}
	}

		SubShader
	{
		LOD 100

		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}

		Pass
	{
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		//Offset - 1, -1
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMaterial AmbientAndDiffuse

		SetTexture[_MainTex]
	{
		Combine Texture * Primary
	}
	}
	}
}

