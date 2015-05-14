struct VS_IN
{
	float3 pos : POSITION; 
	float2 tex : TEXCOORD;
};

struct PS_IN
{
	float4 pos : SV_POSITION;  
	float2 tex : TEXCOORD0;
	float3 worldPos : TEXCOORD1;
};

float4x4 worldViewProj;

Texture2D picture;
SamplerState pictureSampler;

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	
	output.pos = mul(float4(input.pos, 1.0f), worldViewProj);
	output.tex = input.tex;
	output.worldPos = input.pos;
	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	return picture.Sample(pictureSampler, input.tex);
}
