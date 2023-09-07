Shader"Custom/SkyboxBlending"
{
    Properties
    {
        _MainTex1 ("Skybox Texture 1", Cube) = "white" {}
        _MainTex2 ("Skybox Texture 2", Cube) = "white" {}
        _BlendAmount ("Blend Amount", Range(0, 1)) = 0.5
    }
 
    SubShader
    {
        Tags { "Queue"="Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"
 
struct appdata_t
{
    float4 vertex : POSITION;
};
 
struct v2f
{
    float3 pos : TEXCOORD0;
    float4 vertex : SV_POSITION;
};
 
float _BlendAmount;
samplerCUBE _MainTex1;
samplerCUBE _MainTex2;
 
v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.pos = normalize(mul(unity_ObjectToWorld, v.vertex)).xyz;
    return o;
}
 
half4 frag(v2f i) : SV_Target
{
                // 두 개의 텍스처를 섞을 수 있도록 블렌딩합니다.
    half3 blendedColor = lerp(texCUBE(_MainTex1, i.pos), texCUBE(_MainTex2, i.pos), _BlendAmount);
    return half4(blendedColor, 1);
}
            ENDCG
        }
    }
}