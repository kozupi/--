float4x4 xWorldViewProjection;
Texture xColoredTexture;

sampler ColoredTextureSampler = sampler_state
{
	texture = <xColoredTexture> ;
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter = LINEAR;
	AddressU = mirror; 
	AddressV = mirror;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 textureCoordinates: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 textureCoordinates: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(input.Position,  xWorldViewProjection);
	output.textureCoordinates = input.textureCoordinates;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//float4 Color;
//	Color = tex2D(ColoredTextureSampler, input.textureCoordinates.xy);
//	Color += tex2D(ColoredTextureSampler, input.textureCoordinates.xy + (0.01));
	//Color += tex2D(ColoredTextureSampler, input.textureCoordinates.xy - (0.01));
   // return Color/3;
//	float4 color;
//color = tex2D( ColoredTextureSampler, input.textureCoordinates.xy);
//return dot(color, float3(0.3, 0.59, 0.11));
return float4(1,0,0,1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}