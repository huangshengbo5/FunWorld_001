Shader "myshader/relang"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_NoiseTex("Noise (RGB)", 2D) = "white" {}
		_UVScale("uv scale ", Range(0.0, 1.0)) = 1.0
		_NoiseTimeScale("Noise Time Scale ", Range(0,1)) = 1
	}
		SubShader
		{
			Pass{
				CGPROGRAM
				#pragma vertex vert_img  
				#pragma fragment frag  

				#include "UnityCG.cginc"  

				uniform sampler2D _MainTex;
				uniform sampler2D _NoiseTex;
				fixed _UVScale;
				float _NoiseTimeScale;

				fixed4 frag(v2f_img i) : COLOR
				{
					float4 noise = tex2D(_NoiseTex, i.uv - _Time.xy * _NoiseTimeScale);
					float2 offset = noise.xy * _UVScale;
					return tex2D(_MainTex, i.uv + offset);
				}
				ENDCG
			}
		}
	FallBack "Diffuse"

}
