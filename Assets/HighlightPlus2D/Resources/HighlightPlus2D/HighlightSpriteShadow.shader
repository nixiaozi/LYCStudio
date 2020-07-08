Shader "HighlightPlus2D/Sprite/Shadow" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Shadow Color", Color) = (0,0,0,0.2)
}
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    
        // Shadow
        Pass
        {
           	Stencil {
                Ref 1
                Comp NotEqual
                Pass keep 
            }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Offset 1, 1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ POLYGON_PACKING
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

          	fixed2 _Flip;
      		sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _UV;
      		fixed4 _Color;
            
            float2 _Pivot;


            inline float4 UnityFlipSprite(in float4 pos, in fixed2 flip) {
			    return float4(pos.xy * flip, pos.z, 1.0);
			}

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xy -= _Pivot;
                float4 pos = UnityFlipSprite(v.vertex, _Flip);
                pos = UnityObjectToClipPos(pos);

                #ifdef PIXELSNAP_ON
			    pos = UnityPixelSnap (pos);
    			#endif

    			o.pos = pos;
    			#if POLYGON_PACKING
    				o.uv = v.uv;
    			#else
                	o.uv = lerp(_UV.xy, _UV.zw, v.uv);
                #endif
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
			  	#if ETC1_EXTERNAL_ALPHA
    		 	col.a = tex2D (_AlphaTex, i.uv).a;
			  	#endif
			  	col.rgb = max(col.r, max(col.g, col.b));
			  	col *= _Color;
				return col;
            }
            ENDCG
        }

    }
}