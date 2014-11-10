Shader "Hihi" {
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
	        /*float scale = max(0, min(1, timeElapsed - timeStart));
	        scale += cos(timeElapsed - timeStart - 1.0 + timeOffset) * 0.75;
	        v.vertex.x *= scale;
	        v.vertex.y *= scale;
	        v.vertex.z *= scale;*/

	        // Scale Breath
	        //v.vertex.x *= scale * cos(timeElapsed) * 0.1;
	        //v.vertex.y *= scale * cos(timeElapsed) * 0.1;
	        //v.vertex.z *= scale * cos(timeElapsed) * 0.1;

	        // 
	        //float dist = sqrt( ( v.vertex.x * v.vertex.x ) + ( v.vertex.y * v.vertex.y ) + ( v.vertex.z * v.vertex.z ) );

	        // Rotation
	        /*float a = cos(timeElapsed * timeRotationSpeed + timeOffset);
	        float b = sin(timeElapsed * timeRotationSpeed + timeOffset);
	        v.vertex = float4(
	        	(a * v.vertex.x + (-b) * v.vertex.z),
 				v.vertex.y + cos(timeElapsed + timeOffset) * dist * 0.1,
 				(b * v.vertex.x + a * v.vertex.z), 
 				1.0);*/

	        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	        return o;
	    }

	    half4 frag (v2f i) : COLOR
	    {
	    	half4 texcol = tex2D (_MainTex, i.uv);
	    	half4 col = half4(0.0, min(1.0, 0.5 + i.uv.x), 0.0, 1.0); 
            return col;
	    }
	    ENDCG
        }
    }
    Fallback "VertexLit"
}
