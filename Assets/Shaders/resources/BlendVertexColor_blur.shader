// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// unlit, vertex colour, alpha blended
// cull off

Shader "tk2d/BlendVertexColor_blur" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_blurSizeXY("BlurSizeXY", Range(0,20)) = 0
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend SrcAlpha OneMinusSrcAlpha
		LOD 110
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_vct
			#pragma fragment frag_mult 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _blurSizeXY;

			struct vin_vct 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f_vct vert_vct(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag_mult(v2f_vct i) : SV_Target
			{
				float depth= _blurSizeXY*0.0005;

				fixed4 horizontal = tex2D(_MainTex, i.texcoord) * 0.16;
				horizontal += tex2D( _MainTex, i.texcoord + float2(-5.0 * depth, 0.0)) * 0.025;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(5.0 * depth, 0.0)) * 0.025;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(-4.0 * depth, 0.0)) * 0.05;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(4.0 * depth, 0.0)) * 0.05;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(-3.0 * depth, 0.0)) * 0.09;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(3.0 * depth, 0.0)) * 0.09;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(-2.0 * depth, 0.0)) * 0.12;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(2.0 * depth, 0.0)) * 0.12;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(-1.0 * depth, 0.0)) * 0.15;
			    horizontal += tex2D( _MainTex, i.texcoord + float2(1.0 * depth, 0.0)) * 0.15;
			    horizontal = horizontal/2;
			    fixed4 vertical = tex2D(_MainTex, i.texcoord) * 0.16;
				vertical += tex2D( _MainTex, i.texcoord + float2(0.0, -5.0 * depth)) * 0.025;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, 5.0 * depth)) * 0.025;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, -4.0 * depth)) * 0.05;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, 4.0 * depth)) * 0.05;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, -3.0 * depth)) * 0.09;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, 3.0 * depth)) * 0.09;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, -2.0 * depth)) * 0.12;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, 2.0 * depth)) * 0.12;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, -1.0 * depth)) * 0.15;
			    vertical += tex2D( _MainTex, i.texcoord + float2(0.0, 1.0 * depth)) * 0.15;
			    vertical = vertical/2;

				fixed4 col = horizontal + vertical;
			  
			    col += tex2D( _MainTex, i.texcoord-5.0 * depth) * 0.025;    
			    col += tex2D( _MainTex, i.texcoord-4.0 * depth) * 0.05;
			    col += tex2D( _MainTex, i.texcoord-3.0 * depth) * 0.09;
			    col += tex2D( _MainTex, i.texcoord-2.0 * depth) * 0.12;
			    col += tex2D( _MainTex, i.texcoord-1.0 * depth) * 0.15;    
			    col += tex2D( _MainTex, i.texcoord) * 0.16; 
			    col += tex2D( _MainTex, i.texcoord+5.0 * depth) * 0.15;
			    col += tex2D( _MainTex, i.texcoord+4.0 * depth) * 0.12;
			    col += tex2D( _MainTex, i.texcoord+3.0 * depth) * 0.09;
			    col += tex2D( _MainTex, i.texcoord+2.0 * depth) * 0.05;
			    col += tex2D( _MainTex, i.texcoord+1.0 * depth) * 0.025;
			    col = col/2;

				return col * i.color;
			}
			
			ENDCG
		} 
	}
}
