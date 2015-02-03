Shader "_Factorious/Generic Shader" {
    
    /// PROPERTIES ///
    Properties {
    	_Color ("Color", COLOR) = (1,1,1,1)
    }    
    
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
	}
    Fallback "Diffuse"
  }