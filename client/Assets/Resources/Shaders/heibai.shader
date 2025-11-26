// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "myshader/heibai"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_NewTex("New Texture", 2D) = "white" {}
		_Scale("Scale", Range(-10,10.0)) = -8.5
		_Speed("Speed", Range(-50,50.0)) = 1.0
		_Identity("Identity",Range(50,100.0)) = 80
	}
		SubShader{
			Pass{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		sampler2D _MainTex;
		sampler2D _NewTex;
		float _Scale;
		float _Speed;
		float _Identity;

		struct v2f {
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata_img v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			return o;
		}

	float4 frag(v2f i) : COLOR
	{


	//float4 o;
	//half2 uv = i.uv;
	//half r = uv.x;//sqrt(uv.x*uv.x + uv.y*uv.y);
	//half z = cos(_Scale*r + _Time.y * _Speed) / _Identity;
	//o.rgb = tex2D(_MainTex, uv + float2(z,0)).rgb;
	//o.a = 1;
	//return o;

		if (_Scale > 0) {
			float4 o = tex2D(_NewTex, i.uv);
			o.rgb = o.rgb /(1+ _Scale);
			return tex2D(_MainTex, i.uv)*o;
		}
		else if (_Scale < 0) {
			float4 o = tex2D(_NewTex, i.uv);
			o.rgb = o.rgb / (10.1+_Scale);
			o.a = 0;
			return tex2D(_MainTex, i.uv)+o;
		}
		else {
			return tex2D(_MainTex, i.uv);
		}

		//return tex2D(_MainTex, i.uv);
	}



	ENDCG
	}
	}
		FallBack "Diffuse"
}
