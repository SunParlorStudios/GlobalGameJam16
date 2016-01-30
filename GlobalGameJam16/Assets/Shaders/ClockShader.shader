Shader "Unlit/ClockShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Ratio("Clock ratio", Float) = 0.0
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

	struct v2f
	{
		float2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata_full v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.color = v.color;
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed _Ratio;

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = tex2D(_MainTex, i.uv);

	fixed2 center = abs(i.uv * 2.0f - 1.0f);
	float angle = atan2(center.y, center.x);

	if (angle >= _Ratio * 3.14f * 0.5f)
	{
		discard;
	}

	// apply fog
	UNITY_APPLY_FOG(i.fogCoord, col);
	fixed4 final = col * i.color;

	clip(final.a - 0.1f);

	return final;
	}
		ENDCG
	}
	}
}
