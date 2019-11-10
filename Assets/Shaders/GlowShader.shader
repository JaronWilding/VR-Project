Shader "Custom/GlowShader"
{
	Properties
	{
		_Colour("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Speed("Time Multiplier", Range(1,20)) = 1
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent"}
		Blend OneMinusDstColor One
		ZWrite off

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			float4 _Colour;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _Speed;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				float2 newUv = float2(_Time.x + _MainTex_ST.z, _MainTex_ST.w);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.uv = v.uv.xy * _MainTex_ST.xy + newUv;
				return o;
			}

			float fit(float input, float inMin, float inMax, float outMin, float outMax)
			{
				return (input - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
			}

			fixed4 frag(v2f i) : COLOR{
				
				fixed4 col = tex2D(_MainTex, i.uv) * 3;
				col *= _Colour;
				col *= fit(sin(_Time.y * _Speed), -1, 1, 0, 0.7);
				return col;
			}

			ENDCG
		}
	}
}
