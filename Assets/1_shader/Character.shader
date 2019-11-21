Shader "Mobile/Character"
{
	Properties
	{
		_Alpha ("Alpha", Range(0, 1)) = 1
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
		LOD 150

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			Lighting On

		CGPROGRAM
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Alpha;
			
			struct VSOut
			{
				float4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				LIGHTING_COORDS(3, 4)
			};

			VSOut vert(appdata_full v)
			{
				VSOut o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			fixed4 frag(VSOut i) : SV_Target
			{
				fixed4 colorTex = tex2D(_MainTex, i.uv);
				fixed4 o = fixed4(colorTex.rgb * _LightColor0.rgb, colorTex.a * _Alpha);
				return o;
			}
		ENDCG
		}
	}

	FallBack "Mobile/Diffuse"
}
