Shader "Custom/DualColorReplace"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _ColorA ("Color for 70%", Color) = (1,0,0,1)
        _ColorB ("Color for 100%", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ColorA;
            float4 _ColorB;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                float lum = tex.r;
                
                if (tex.r == tex.g && tex.r == tex.b && tex.a > 0.5 && lum > 0.1){
                    if (lum > 0.45)
                        return _ColorA;
                    else
                        return _ColorB;
                }
                else
                    return tex;
            }
            ENDCG
        }
    }
}
