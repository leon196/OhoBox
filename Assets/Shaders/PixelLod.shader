

Shader "PixelLod" {
    Properties {
        _MainTex ("Base (RGBA)", 2D) = "white" {}
    }
    SubShader {


        Pass {
            AlphaTest Greater .5
            Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
            Cull off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            
            uniform float timeElapsed;

            uniform float slider1;
            uniform float slider2;

            uniform float spiner1;
            uniform float spiner2;
            uniform float spiner3;

            float4 frag(v2f_img sp) : COLOR 
            {
                float2 uv = sp.uv.xy;
                uv.y = 1.0 - uv.y;
                
                float2 target = float2(0.5, 0.5);
                //float2(cos(timeElapsed) * 0.5 + 0.5, sin(timeElapsed) * 0.125 + 0.75);
                
                //float2 mouse = float2(iMouse.xy / iResolution.xy);
                //mouse.y = 1.0 - mouse.y;
                
                //target = mouse;
                
                float minRange = 1.0 + floor(slider1 * 16.0);
                float maxRange = 8.0 + floor(slider2 * 8.0);
                
                float area = 2.0 + floor(spiner1 * 32.0);
                float dist = distance(floor(uv*area)/area, target);
                
                dist += spiner2;
                dist = max(0.0, min(1.0, 1.0 - dist));
                
                dist = dist * dist;
                
                float details = minRange + floor(dist * maxRange);
                
                details = pow(2.0, details);
                
                uv = floor(uv * details) / details;

                return tex2D (_MainTex, uv);
            }

            ENDCG
        }
    }
}

