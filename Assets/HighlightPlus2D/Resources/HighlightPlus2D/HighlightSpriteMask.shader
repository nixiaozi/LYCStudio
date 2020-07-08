Shader "HighlightPlus2D/Sprite/Mask" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _CutOff ("Apha CutOff", Float) = 0.05
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
}
    SubShader
    {
        Tags { "Queue"="Transparent-10" "RenderType"="Transparent" }

        // Create mask
        Pass
        {
			Stencil {
                Ref 1
                Comp always
                Pass replace
            }
            ColorMask 0
            ZWrite Off
            Offset -1, -1
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ POLYGON_PACKING
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            fixed2 _Flip;
            fixed _CutOff;
            float4 _UV;
            float2 _Pivot;
            
			inline float4 UnityFlipSprite(in float4 pos, in fixed2 flip) {
			    return float4(pos.xy * flip, pos.z, 1.0);
			}

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v, out float4 pos : SV_POSITION)
            {
                v.vertex.xy -= _Pivot;
            	pos = UnityFlipSprite(v.vertex, _Flip);
                pos = UnityObjectToClipPos(pos);

                #ifdef PIXELSNAP_ON
			    pos = UnityPixelSnap (pos);
    			#endif

                v2f o;
    			#if POLYGON_PACKING
    				o.uv = v.uv;
    			#else
                	o.uv = lerp(_UV.xy, _UV.zw, v.uv);
                #endif
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
            	fixed4 color = tex2D(_MainTex, i.uv);
				#if ETC1_EXTERNAL_ALPHA
    			color.a = tex2D (_AlphaTex, i.uv).a;
				#endif
            	clip(color.a - _CutOff);
            	return 0;
            }
            ENDCG
        }

    }
}