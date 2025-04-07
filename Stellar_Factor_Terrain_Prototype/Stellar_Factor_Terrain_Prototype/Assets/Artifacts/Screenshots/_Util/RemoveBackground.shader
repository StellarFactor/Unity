Shader "Custom/Remove Background" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskColor ("Background Color to Mask", Color) = (0.2, 0.2, 0.2, 1)
        _Tolerance ("Color accuracy tolerance", Float) = .0001
    }
    SubShader {
        Tags {
            "Queue" = "Overlay"        // Ensures it renders in the UI layer
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "IsOverlay" = "True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MaskColor;
            float _Tolerance;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Check if the pixel color is close to the mask color
                if (distance(col.rgb, _MaskColor.rgb) >= _Tolerance)
                {
                    // Keep only the matching color
                    col.a = 0; // Make everything else transparent
                }

                return col;
            }
            ENDCG
        }
    }
}