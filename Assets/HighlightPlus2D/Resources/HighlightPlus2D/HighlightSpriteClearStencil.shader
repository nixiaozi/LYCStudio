Shader "HighlightPlus2D/Sprite/ClearStencil" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
}
    SubShader
    {
        Tags { "Queue"="Transparent+102" "RenderType"="Transparent" "DisableBatching"="True" }

        // Create mask
        Pass
        {
			Stencil {
                Ref 1
                Comp always
                Pass zero
            }
            ColorMask 0
            ZWrite Off
            Offset -1, -1
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            void vert (appdata v, out float4 pos : SV_POSITION)
            {
                v.vertex.xy *= 2.0;
                pos = UnityObjectToClipPos(v.vertex);
            }
            
            fixed4 frag (float4 position : SV_POSITION) : SV_Target
            {
                return 0;
            }
            ENDCG
        }

    }
}