Shader "Custom/ScrollingX"
{
	Properties
	{
		 _Color("Color", Color) = (1,1,1,1)				// 颜色
		_MainTex("MainTex(RGBA)", 2D) = "green" {}     // 纹理    
	_ScrollX("Base layer Scroll Speed",Float) = 1.0    // 滚动速度

		_Mutiplier("Layer Mutiplier", Float) = 1         //整体亮度
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Overlay" "IgnoreProjector" = "True" }		// 设置材质Model
		LOD 100
		Cull Off Lighting Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		Tags{ "LightMode" = "ForwardBase" }
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag


#include "UnityCG.cginc"

		struct a2v
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _ScrollX;
	float _Mutiplier;
	fixed4 _Color;

	v2f vert(a2v v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex) + frac(float2 (_ScrollX, 0.0) * -(_Time.y));		// 控制波纹方向

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 c = tex2D(_MainTex, i.uv.xy);
		c.rgb *= _Color;		
		

	return c;
	}
		ENDCG
	}
	}
		FallBack "VertexLit"
}

