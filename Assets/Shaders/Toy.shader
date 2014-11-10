

Shader "Hoho" {
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
            	uv.x = abs(cos(slider1 + uv.x));
            	uv.y = abs(cos(slider2 + uv.y));

            	int lod = 1.0 + floor(spiner3 * 16.0);
            	lod = pow(2.0, lod);
            	sp.uv = floor( sp.uv * lod ) / lod;

	    		half4 texcol = tex2D (_MainTex, sp.uv);

                return texcol * float4(uv.x, uv.y, 1.0 - uv.y, 1.0);
            }

            ENDCG
        }
    }
}

