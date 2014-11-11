Shader "RandomShades" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
    }
    SubShader {


        Pass {
            Tags {"Queue"="Opaque"}

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            
            uniform float4 _Color;
            
            uniform float timeElapsed;

            uniform float slider1;
            uniform float slider2;

            uniform float spiner1;
            uniform float spiner2;
            uniform float spiner3;

            float rand(float2 co){
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }

            float4 vert(appdata_base v) : POSITION {
                return mul (UNITY_MATRIX_MVP, v.vertex);
            }

            fixed4 frag(float4 sp:WPOS) : COLOR 
            {
                float2 uv = sp.xy;
                float ratio = _ScreenParams.y / _ScreenParams.x;
                float speed = 1.0 + spiner1 * 10.0;
                float zoom = 1.0 + spiner2 * 10.0;
                float details = 1.0 + floor(spiner3 * 64.0);

                float4 color = float4(hsv2rgb(float3(slider1, 1.0, 1.0)), 1.0);
                float random = slider2;

                //uv.y *= ;

/*
                uv.x += timeElapsed * (spiner3 - 0.5) * 2.0;

                uv.xy = floor(uv.xy * details) / details;
                return float4(hsv2rgb(float3(uv.x / zoom, 1.0, 1.0)), 1.0);*/

                uv.x /= uv.y / uv.x;
                if (uv.y > 0.5) {
                    uv.y -= timeElapsed * speed;
                } else {
                    uv.y += timeElapsed * speed;
                }

                uv.xy = floor(uv.xy * details) / details;
                return color * rand(uv.xy);

            }
/*
            float4 frag(v2f_img sp) : COLOR 
            {
                float2 uv = gl_FragCoord.xx;
                uv.y = 1.0 - uv.y;

                uv.xy = floor(uv.xy * 32.0) / 32.0;

                float4 color = _Color * rand(uv.xy);

                return color;
            }
*/
            ENDCG
        }
    }
}

