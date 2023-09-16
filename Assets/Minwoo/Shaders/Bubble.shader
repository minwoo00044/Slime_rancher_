Shader"Custom/Bubble"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert alpha:fade
        #pragma target 3.0

float getAddPos(float pos, int offset)
{
    float speed = 8.0 + offset * 4.0;
    return sin(pos * 10 + _Time.y * speed) * 0.02;
}

void vert(inout appdata_full v)
{
    v.vertex.x += getAddPos(v.vertex.x, 0);
    v.vertex.y += getAddPos(v.vertex.y, 1);
    v.vertex.z += getAddPos(v.vertex.z, 2);
}

struct Input
{
    float3 viewDir;
    float3 worldPos;
};

half _Glossiness;
half _Metallic;

void surf(Input IN, inout SurfaceOutputStandard o)
{
    //float3 col = sin(_Time.w + IN.worldPos * 10) * 0.3 + 0.7;
    float3 col = float3(0, 0, 1);;
    o.Albedo = col;

    float rim = dot(o.Normal, IN.viewDir);
    o.Alpha = saturate(pow(1 - rim, 1) + 0.1);

    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
}

        ENDCG
    }
FallBack"Diffuse"
}