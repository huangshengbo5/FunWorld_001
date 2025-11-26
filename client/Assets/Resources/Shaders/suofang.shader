Shader "myshader/suofang"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Speed("Rotate Speed",Range(0,4)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag			
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float _Speed;

				struct a2v {
					float4 vertex:POSITION;
					float4 texcoord:TEXCOORD;
				};
				struct v2f {
					float4 pos:POSITION;
					float4 uv:texcoord;
				};

				v2f vert(a2v v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					return o;
				}

				fixed4 frag(v2f i) :SV_Target{
					////扭曲效果
					fixed2 uv = i.uv - fixed2(0.5,0.5);
					float angle = _Speed * 0.1745 / (length(uv) + 0.1);//加0.1是放置length（uv）为0
					float angle2 = angle * _Time.y;
					uv = float2(uv.x*cos(angle2) - uv.y*sin(angle2),uv.y*cos(angle2) + uv.x*sin(angle2));
					uv += fixed2(0.5,0.5);
					fixed4 c = tex2D(_MainTex,uv);
					return c;

					//缩放
					//float4 uv = i.uv;
					//uv.x*= _Speed;
					//uv.w=0.1;
					//return tex2D(_MainTex,uv);

					//旋转
					//float2 uv=i.uv.xy-float2(0.5,0.5);
					//uv=float2(uv.x*cos(_Speed*_Time.y)-uv.y*sin(_Speed*_Time.y),uv.y*cos(_Speed*_Time.y) + uv.x*sin(_Speed*_Time.y));
					//uv+=float2(0.5,0.5);
					//fixed4 lateUV = tex2D(_MainTex,uv);
					//return lateUV;

					//平移
					//return tex2D(_MainTex,i.uv - _Time.x*fixed2(2,0)* _Speed);
				}

				ENDCG
			}
		}

}
