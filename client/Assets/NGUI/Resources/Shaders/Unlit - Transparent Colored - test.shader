Shader "Unlit/Transparent Colored test"
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
			"DisableBatching" = "True"
		}

//		1、Blend SrcAlpha OneMinusSrcAlpha 正常模式(透明度混合)
//这里写图片描述
//2、Blend OneMinusDstColor One 柔和相加(soft Additive)
//这里写图片描述
//3、Blend DstColor Zero 正片叠底(Multiply)相乘
//这里写图片描述
//4、Blend DstColor SrcColor 两倍相乘(2X Multiply)
//这里写图片描述
//5、 变暗
//
//BlendOp Min
//Blend One One
//1
//2
//这里写图片描述
//6、变亮
//
//BlendOp Max
//Blend One One
//1
//2
//这里写图片描述
//7、Blend OneMinusDstColor One 滤色
//这里写图片描述
//8、Blend One One 线性变淡
//这里写图片描述

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
//			Blend SrcAlpha OneMinusSrcAlpha  //正常模式(透明度混合)
		//          Blend SrcAlpha OneMinusSrcAlpha
//          Blend OneMinusDstColor One //柔和相加(soft Additive)
//          Blend DstColor Zero  //正片叠底 (Multiply)相乘
//          Blend DstColor SrcColor  //两倍相乘 (2X Multiply)
          BlendOp Max
//          Blend One One    //线性变淡
          Blend OneMinusDstColor One

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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f o;

			v2f vert(appdata_t v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				return tex2D(_MainTex, IN.texcoord) * IN.color;
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
					"DisableBatching" = "True"
				}

				Pass
				{
					Cull Off
					Lighting Off
					ZWrite Off
					Fog { Mode Off }
					Offset -1, -1
				//ColorMask RGB
//          Blend SrcAlpha OneMinusSrcAlpha
//          Blend OneMinusDstColor One
//          Blend DstColor Zero
//          Blend DstColor SrcColor
          BlendOp Max
//         Blend One One
          Blend OneMinusDstColor One


				ColorMaterial AmbientAndDiffuse

				SetTexture[_MainTex]
				{
					Combine Texture * Primary
				}
			}
			}
}
