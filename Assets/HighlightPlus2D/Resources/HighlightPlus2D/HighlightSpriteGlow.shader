Shader "HighlightPlus2D/Sprite/Glow" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _NormalsTex ("2D Normals", 2D) = "white" {}
    _Glow ("Glow", Vector) = (1, 0.025, 0.75, 0.5)
    _Glow2 ("Glow2", Vector) = (0.01, 1, 0.5, 0)
    _GlowColor ("Glow Color", Color) = (1,1,1)
    _CutOff ("Apha CutOff", Float) = 0.05
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
}
    SubShader
    {
        Tags { "Queue"="Transparent-9" "RenderType"="Transparent" }
      
        // Glow passes
        Pass
        {
        	Stencil {
                Ref 1
                Comp NotEqual
                Pass keep 
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

		    fixed2 _Flip;
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;
            float4 _AlphaTex_TexelSize;
            float4 _UV;
            fixed _CutOff;
            
            float2 _Pivot;
            float3 _Geom;
            
            float4 _Glow; // x = intensity, y = width, z = magic number 1, w = magic number 2
            float3 _Glow2; // x = outline width, y = glow speed, z = dither on/off
            fixed4 _GlowColor;

            inline float4 UnityFlipSprite(in float4 pos, in fixed2 flip) {
			    return float4(pos.xy * flip, pos.z, 1.0);
			}

            v2f vert (appdata v, out float4 pos : SV_POSITION)
            {
                v2f o;

                v.vertex.xy -= _Pivot;
                pos = UnityFlipSprite(v.vertex, _Flip);
                pos = UnityObjectToClipPos(pos);

                float z = pos.w;
                float offset = z * (_Glow.y * (1.0 + 0.25 * sin(_Time.w * _Glow2.y)));
                float4 posZero = UnityObjectToClipPos(float4(_Geom.xy,0,1));
                float2 dir = pos.xy - posZero.xy;
                dir.y *= _Geom.z;
                pos.xy += normalize(dir) * offset;

                #ifdef PIXELSNAP_ON
			    pos = UnityPixelSnap (pos);
    			#endif

    			#if POLYGON_PACKING
    				o.uv = v.uv;
    			#else
                	o.uv = lerp(_UV.xy, _UV.zw, v.uv);
                #endif
                o.color = _GlowColor;
                o.color.a = _Glow.x;
                return o;
            }
            
            fixed4 frag (v2f i, UNITY_VPOS_TYPE scrPos : VPOS) : SV_Target
            {
                fixed4 color = i.color;
                float2 screenPos = floor(scrPos.xy * _Glow.z) * _Glow.w;
                color.a *= saturate(_Glow2.z + frac(screenPos.x + screenPos.y));

                fixed4 neighbours;
				#if ETC1_EXTERNAL_ALPHA
                	neighbours.x = tex2D(_AlphaTex, i.uv + fixed2(0, _AlphaTex_TexelSize.y)).a;
                	neighbours.y = tex2D(_AlphaTex, i.uv - fixed2(0, _AlphaTex_TexelSize.y)).a;
                	neighbours.z = tex2D(_AlphaTex, i.uv + fixed2(_AlphaTex_TexelSize.x, 0)).a;
                	neighbours.w = tex2D(_AlphaTex, i.uv - fixed2(_AlphaTex_TexelSize.x, 0)).a;
				#else
                	neighbours.x = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
                	neighbours.y = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
                	neighbours.z = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
                	neighbours.w = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;
                #endif

                neighbours = saturate(neighbours - _CutOff);
                return color * any(neighbours);
            }
            ENDCG
        }
 
    }
}