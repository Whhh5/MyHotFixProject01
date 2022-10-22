Shader "Example/SimpleUnlitColor"
{
    Properties
    {
        [NoScaleOffset][MainTexture] _MainTex ("_MainTex", 2D) = "white" {}
        _Color_1 ("_Color_1", Color) = (0,0,0,1)
        _Temp ("_Temp", float) = 0.7
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode" = "explacment1" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"



            half4 _Color_1;
            float _Temp;

            struct appdata
            {
                float4 vertex   :   POSITION;
            };  

            struct v2f
            {
                float4  vertex   :   SV_POSITION;
                float   depth    :   DEPTH;
            };

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = -(mul(UNITY_MATRIX_MV, v.vertex).z) * _ProjectionParams.w;

                return o;
            }

            fixed4 frag (v2f i) :   SV_Target
            {
                //fixed4 col;

                float invert = 1 - i.depth;

                //if (abs(i.depth - _Temp / 2) < 0.005)
                //    return fixed4(invert * 2, 0.2, 0.2, 1);
                //else
                //    return fixed4(invert, 0.3, 0.3, 1);

                return fixed4(invert,invert,invert,1) ;
            }

            ENDCG
        }
    }
}