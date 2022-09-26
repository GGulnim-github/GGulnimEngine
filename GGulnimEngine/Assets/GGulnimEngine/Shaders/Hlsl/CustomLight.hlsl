#ifndef CUSTOMLIGHT_INCLUDED
#define CUSTOMLIGHT_INCLUDED

struct CustomLightingData
{
    // Position and orientation
    float3 normalWS;
    float3 viewDirectionWS;

    // Surface attributes
    float3 albedo;
    float smoothness;
};

float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
float3 CustomLightHandling(CustomLightingData d, Light light)
{
    float3 radiance = light.color;

    float diffuse = saturate(dot(d.normalWS, light.direction));
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;

    float3 color = d.albedo * radiance * (diffuse + specular);

    return color;
}
#endif
float3 CalculateCustomLighting(CustomLightingData d) 
{
    #ifdef SHADERGRAPH_PREVIEW
    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.normalWS, lightDir)) +
        pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
    #else
      // Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
    Light mainLight = GetMainLight();

    float3 color = 0;
    // Shade the main light
    color += CustomLightHandling(d, mainLight);

    return color;
    #endif
}

void CalculateCustomLight_float(float3 Normal, float3 ViewDirection, float3 albedo, float smoothness, out float3 Color)
{    
    CustomLightingData d;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = albedo;
    d.smoothness = smoothness;

    Color = CalculateCustomLighting(d);
}
#endif