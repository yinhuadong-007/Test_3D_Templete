Shader "Custom/Test_ShaderMask"
{
    Properties
    {
        // _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "write" {}
        // _Radius ("Radius", float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Blend Off  //默认关闭
        // Blend SrcAlpha OneMinusSrcAlpha // 传统透明度
        // Blend One OneMinusSrcAlpha // 预乘透明度
        // Blend One One // 加法
        // Blend OneMinusDstColor One // 软加法
        // Blend DstColor Zero // 乘法
        // Blend DstColor SrcColor // 2x 乘法

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float2 _TouchUV;
            float _Radius;
            float _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // fixed4 col = _Color;

                float2 tpos = i.vertex;
                float dis = distance(tpos, _TouchUV);
                if(dis < _Radius){
                    col.a = 0;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
