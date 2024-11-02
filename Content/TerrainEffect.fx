#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
float3 LightDirection = float3(1, 1, 0);
float TextureTiling = 1;
float3 Tint;

texture2D BaseTexture;
sampler2D BaseTextureSampler = sampler_state
{
    Texture = <BaseTexture>;
    AddressU = Wrap;
    AddressV = Wrap;
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 UV : TEXTCOORD0;
    float3 Normal : NORMAL0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
	output.Position = mul(input.Position, WorldViewProjection);
    output.Normal = normalize(input.Normal);
    output.UV = input.UV;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float light = saturate(dot(normalize(LightDirection), input.Normal) + 0.1f);
    float3 tex = tex2D(BaseTextureSampler, input.UV * TextureTiling);
    
	return float4(light * max(Tint, tex), 1.f);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};