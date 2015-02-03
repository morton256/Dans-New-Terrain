Shader "_Factorious/Outline Shader" {
    
    /// PROPERTIES ///
    Properties {
    	_Color ("Color", COLOR) = (1,1,1,1)
    	
    	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005	
    }
    
	CGINCLUDE
	#include "UnityCG.cginc"
	 
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	 
	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};
	 
	uniform float _Outline;
	uniform float4 _OutlineColor;
	 
	v2f vert(appdata v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	 
		float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);
	 
		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG 
    
   //SUB SHADER ///
    SubShader {
      CGPROGRAM
      #pragma surface surf SimpleLambert
      
       // Lighting for the material
      	half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
             	half NdotL = dot (s.Normal, lightDir);
             	half4 c;
              	c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
             	c.a = s.Alpha;
             	return c;
             	}

      	fixed4 _Color;
      				
		struct Input {
			float4 color : COLOR;
		};
			     	 
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
		}
			      		      	      	     	
      ENDCG
      
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset 50,50
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
    }
    
    Fallback "Diffuse"
  }