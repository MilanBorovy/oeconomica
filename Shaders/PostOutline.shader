Shader "Custom/Post Outline"
{
	Properties
	{
		_MainTex("Main Texture",2D) = "black"{}
		_Color("Color", Color) = (1, 1, 1) //Color
	}
	SubShader
	{
		Color [_Color]
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM

				float3 _Color;
				sampler2D _MainTex;

				//Conversion pixel - texel
				float2 _MainTex_TexelSize;

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uvs : TEXCOORD0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;

					//Multiplication of vertices by MVP matrix
					o.pos = mul(UNITY_MATRIX_MVP,v.vertex);

					//UVs fix
					o.uvs = o.pos.xy / 2 + 0.5;

					return o;
				}


				half4 frag(v2f i) : COLOR
				{
					//Width of outline
					int NumberOfIterations = 9;

					//Split texel size into smaller words
					float TX_x = _MainTex_TexelSize.x;
					float TX_y = _MainTex_TexelSize.y;

					//Color intesity based on surrounding intensities
					float ColorIntensityInRadius;

					//If something already exists underneath the fragment, discard the fragment.
					if (tex2D(_MainTex,i.uvs.xy).r>0)
					{
						discard;
					}

					//Horizontal
					for (int k = 0; k<NumberOfIterations; k += 1)
					{
						//Vertical
						for (int j = 0; j<NumberOfIterations; j += 1)
						{
							//Color by pixels in area
							ColorIntensityInRadius += tex2D(
								_MainTex,
								i.uvs.xy + float2
								(
									(k - NumberOfIterations / 2)*TX_x,
									(j - NumberOfIterations / 2)*TX_y
								)
							).r / NumberOfIterations;
						}
					}
					//Output color;
					return ColorIntensityInRadius*float4(_Color.r,_Color.g,_Color.b,0.1);
				}

			ENDCG

		}
		//End pass        
	}
	//End subshader
}
//End shader