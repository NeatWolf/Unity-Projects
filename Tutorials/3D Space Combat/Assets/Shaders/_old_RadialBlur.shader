Shader "Shaders/_RadialBlur"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "" {}
		_Center("Screen Center", Vector) = (0, 0, 0, 0)
		_BlendAmount("Blur", Float) = 1
		_Samples("Number of Samples", Int) = 8
	}

		// Shader code pasted into all further CGPROGRAM blocks
		CGINCLUDE

#include "UnityCG.cginc"

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;

	float2 _Center;
	float _BlendAmount;
	half _Samples;

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		half2 coordinates = i.uv - _Center;
		half4 color = half4(0,0,0,0);
		half scale;

		for (int j = 0; j < _Samples; j++)
		{
			scale = (_BlendAmount * (j / (_Samples - 1))) + 1;
			color += tex2Dlod(_MainTex, half4(coordinates * scale + _Center, 0.0, 0.0));
		}

		return color / (float)_Samples;
	}

		ENDCG

		Subshader
	{
		Blend One Zero
			Pass{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

			ENDCG
		} // Pass
	} // Subshader

	Fallback off

} // shader
