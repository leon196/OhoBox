Shader "WormShit" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Pass {

   		Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
	    Cull off
	    ZWrite Off
	    Blend SrcAlpha OneMinusSrcAlpha

	    CGPROGRAM

	    #pragma vertex vert
	    #pragma fragment frag
	    #include "UnityCG.cginc"

	    float4 _Color;
        sampler2D _MainTex;

        // Global
        uniform float timeElapsed;
        uniform float timeRotationSpeed;

        // Local
        uniform float timeOffset;
        uniform float timeStart;
        uniform float3 angleOffset;

	    struct v2f {
	        float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
	    };

        float4 _MainTex_ST;

	    v2f vert (appdata_base v)
	    {
	        v2f o;

	        // Scale Growth
	        float scale = 1.0 + cos(timeElapsed - timeStart + timeOffset) * 0.25;
	        v.vertex.x *= scale;
	        v.vertex.y *= scale;
	        v.vertex.z *= scale;

	        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	        return o;
	    }

	    half4 frag (v2f i) : COLOR
	    {
	    	half4 texcol = tex2D (_MainTex, i.uv);
	    	half4 col = half4(min(1.0, 0.5 + i.uv.x * i.uv.y), 0.0, 0.0, 1.0); 
            return col;
	    }
	    ENDCG
        }
    }
    Fallback "VertexLit"
}
