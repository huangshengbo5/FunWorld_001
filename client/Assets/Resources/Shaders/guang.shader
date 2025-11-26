// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "myshader/guang"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_OutlineColor("Outline Color", Color) = (1,1,0,1)
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

		Cull Off
		Lighting Off
		ZWrite Off
		//Offset - 1, -1
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

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
		half4 color : COLOR;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		half4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.color = v.color;
		return o;
	}

	sampler2D _MainTex;
	uniform float4 _OutlineColor;
	fixed4 frag(v2f i) : COLOR
	{
		half4 col = i.color;
		col.a *= tex2D(_MainTex, i.uv).a;
		half4 outCol = _OutlineColor;
		col.a *= outCol.a;
		col.rgb = float3(outCol.r, outCol.g, outCol.b);
		//	col.rgb = float3(1.0, 0.0, 0.0);
		return col;
	}
		ENDCG
	}
	}

}