Shader "HighlightPlus2D/Sprite/SeeThrough" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _CutOff ("Apha CutOff", Float) = 0.05
    _SeeThrough ("See Through", Range(0,1)) = 0.8
    _SeeThroughTintColor ("See Through Tint Color", Color) = (1,0,0,0.8)
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
}
    SubShader
    {
        Tags { "Queue"="Transparent+101" "RenderType"="Transparent" }
   
        // See through effect
        Pass
        {
            Stencil {
                Ref 1
                Comp NotEqual
                Pass keep 
            }
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
            };

           	fixed2 _Flip;
           	fixed _CutOff;
            sampler2D _MainTex;
            float4 _UV;
            sampler2D _AlphaTex;
            
            float2 _Pivot;

            fixed _SeeThrough;
            fixed4 _SeeThroughTintColor;

            inline float4 UnityFlipSprite(in float4 pos, in fixed2 flip) {
			    return float4(pos.xy * flip, pos.z, 1.0);
			}

            v2f vert (appdata v, out float4 pos : SV_POSITION)
            {
                v2f o;
                v.vertex.xy -= _Pivot;
                pos = UnityFlipSprite(v.vertex, _Flip);
                pos = UnityObjectToClipPos(pos);

                #ifdef PIXELSNAP_ON
			    pos = UnityPixelSnap (pos);
    			#endif

    			#if POLYGON_PACKING
    				o.uv = v.uv;
    			#else
                	o.uv = lerp(_UV.xy, _UV.zw, v.uv);
                #endif
                return o;
            }
            
            fixed4 frag (v2f i, UNITY_VPOS_TYPE scrPos : VPOS) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                #if ETC1_EXTERNAL_ALPHA
                col.a = tex2D (_AlphaTex, i.uv).a;
                #endif
                clip(col.a - _CutOff);
                col.rgb = lerp(col.rgb, _SeeThroughTintColor.rgb, _SeeThroughTintColor.a);
                col.rgb += frac(scrPos.y * _Time.w) * 0.1;
                col.a = _SeeThrough;
            	col.a *= (scrPos.y % 2) - 1.0;
                return col;
            }
            ENDCG
        }

    }
}