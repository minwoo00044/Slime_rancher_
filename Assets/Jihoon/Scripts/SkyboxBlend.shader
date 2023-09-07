Shader"Custom/SkyboxBlend" {
    Properties {
        _SkyboxTexture1 ("Skybox Texture 1", Cube) = "" {}
        _SkyboxTexture2 ("Skybox Texture 2", Cube) = "" {}
        _BlendAmount ("Blend Amount", Range(0, 1)) = 0.5
    }
 
    SubShader {
        Tags { "Queue" = "Background" }
        Pass {
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
    float4 pos : SV_POSITION;
    float3 tex : TEXCOORD0;
};
 
float4x4 unity_Skybox : register(C0);
 
v2f vert(appdata_t v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.tex = v.vertex.xyz;
    return o;
}
 
samplerCUBE _SkyboxTexture1;
samplerCUBE _SkyboxTexture2;
float _BlendAmount;
 
half4 frag(v2f i) : SV_Target
{
    half4 skyColor1 = texCUBE(_SkyboxTexture1, i.tex);
    half4 skyColor2 = texCUBE(_SkyboxTexture2, i.tex);
    half4 finalColor = lerp(skyColor1, skyColor2, _BlendAmount);
    return finalColor;
}
            ENDCG
        }
    }
}