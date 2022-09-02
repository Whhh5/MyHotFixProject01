// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Shader_Wave"
{
    Properties
    {
        _StartColor ("start color", Color) = (0,0,0,0)
        _EndColor ("end color", Color) = (0,0,0,0)
        _MaxDircetion ("max dircetion", float) = 0
        _MaxHeright ("max height", float) = 0
        _Speed ("speed", float) = 0
        _HeightBottom ("height bottom", float) = 0
    }
    SubShader
    {
        GrabPass{"_bTexture"}
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite On
            ZTest LEqual


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            float4 _StartColor;
            float4 _EndColor;
            float _MaxDircetion;
            float _MaxHeright;
            float _Speed;
            float _HeightBottom;
            sampler2D _bTexture;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv0 : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f vert(MeshData _meshData)
            {
                v2f _modeData;
                float dir = pow(
                    pow(_meshData.vertex.x, 2) +
                    pow(_meshData.vertex.y, 2) +
                    pow(_meshData.vertex.z, 2)
                    , 0.5);

                _MaxDircetion = abs(_MaxDircetion) * abs(_Speed);
                dir = min(dir, _MaxDircetion);
                dir = max(dir, 0);
                float heightPercent = cos(dir + _Time.y * _Speed);

                float heightAttenuation = 1 - dir / _MaxDircetion;

                _meshData.vertex.x += _meshData.normal.x * heightPercent * heightAttenuation * _MaxHeright;
                _meshData.vertex.y += _meshData.normal.y * heightPercent * heightAttenuation * _MaxHeright;
                _meshData.vertex.z += _meshData.normal.z * heightPercent * heightAttenuation * _MaxHeright;

                _meshData.vertex.y -= (_meshData.vertex.y / _MaxHeright) * _HeightBottom;


                _modeData.uv0 = _meshData.uv0;
                _modeData.vertex = UnityObjectToClipPos(_meshData.vertex);
                _modeData.normal = UnityObjectToWorldNormal(_meshData.normal);
                //_modeData.vertex = _meshData.vertex;
                return _modeData;
            }

            fixed4 frag(v2f _modeData) : SV_Target
            {
                //float4 localVertex = unity_WorldToObject * _modeData.vertex;

                const float t = cos(_Time);
                fixed4 _col = lerp(_StartColor, _EndColor, t);

                
                //_col *= _modeData.uv0.x;
                //_col = min(_col,255);
                //_col = max(_col, 0);
                //_col += localVertex;
                fixed4 screenColor = tex2D(_bTexture, _modeData.uv0.xy);
                return _col * screenColor;
            }
            ENDCG
        }
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest GEqual
            Blend One One


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _RimColor =(1,1,1,1);
            float _RimPower = 1;
            float _RimIntensity = 1;

            struct a2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
            };

            struct v2f
            {
                float4 pos:SV_POSITION;
                float4 worldPos:TEXCOORD0;
                float3 worldNormal:TEXCOORD1;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i):SV_TARGET
            {
                
                fixed3 worldNormalDir = normalize(i.worldNormal);
                fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed rim = 1 - saturate(dot(worldNormalDir, worldViewDir));

                fixed4 col = _RimColor.xyzw * pow(rim, _RimPower) * _RimIntensity;
                return col;
            }
            ENDCG
        }
    }
}