// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineShader"
{
	Properties
	{
		_OutlineColour("Outline Colour", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1.0, 5.0)) = 1.0
    }

	CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata 
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
			float4 color : COLOR;
		};

		uniform float _OutlineWidth;
		uniform float4 _OutlineColour;


		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);

			o.pos.xy += offset * o.pos.z * _OutlineWidth;
			o.color = _OutlineColour;
			return o;
		}

	ENDCG
    SubShader
    {
		Tags { "Queue" = "Transparent" }
		Pass
		{
			Name "BASE"
			Cull Back
			Blend Zero One

			SetTexture[_OutlineColour] 
			{
				ConstantColor(0,0,0,0)
				Combine constant
			}
		}
		Pass 
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			
			Blend One OneMinusDstColor //Soft Add

			CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag


				half4 frag(v2f i) : COLOR
				{
					return i.color;
				}

			ENDCG
		}
    }
		Fallback "Diffuse"
}
