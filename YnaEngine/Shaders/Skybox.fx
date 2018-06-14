// Matrix
const float E = 2.71828;
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 EyePosition;

#if SM4
bool FogEnabled;
// Fog [0] => Mode [1] => Density [2] => Start [3] => End
float4 FogData;
float3 FogColor;
#endif

texture MainTexture;
samplerCUBE SkyboxSampler = sampler_state
{
    Texture = <MainTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};

#if SM4
float CalcFogFactor(float camDistance)
{
    float fogCoeff = 1.0;
    int mode = (int) FogData.x;
    float density = FogData.y;
    float start = FogData.z;
    float end = FogData.w;
	
    if (mode == 1)
        fogCoeff = (end - camDistance) / (end - start);
    else if (mode == 2)
        fogCoeff = 1.0 / pow(E, camDistance * density);
    else if (mode == 3)
        fogCoeff = 1.0 / pow(E, camDistance * camDistance * density * density);

    if (mode > 0)
        fogCoeff = clamp(fogCoeff, 0.0, 1.0);

    return fogCoeff;
}
#endif

struct VertexShaderInput
{
#if SM4
	float4 Position : SV_Position;
#else
    float4 Position : POSITION0;
#endif
    float3 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float3 UV : TEXCOORD0;
#if SM4
    float FogDistance : FOG;
#endif
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.UV = worldPosition.xyz - EyePosition;
#if SM4
    output.FogDistance = distance(worldPosition.xyz, EyePosition);
#endif
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 diffuse = texCUBE(SkyboxSampler, normalize(input.UV));

#if SM4
    if (FogEnabled == true)
	{
		if (FogData.x > 0)
		{
			float fog = CalcFogFactor(input.FogDistance);
			return float4((fog * diffuse + (1.0 - fog)) * FogColor, 1.0);
		}

		return float4(diffuse.xyz, 1.0);
	}
#endif

    return diffuse;
}

technique Skybox
{
    pass AmbientPass
    {
#if SM4
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
#else
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
#endif
    }
}