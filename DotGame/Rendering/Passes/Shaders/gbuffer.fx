struct VS_IN
{
	float3 pos : POSITION; 
	float3 normal : NORMAL;
	float2 tex : TEXCOORD;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float3 normal : NORMAL;
	float2 tex : TEXCOORD0;
};


struct PS_OUT
{
	float4 diffuse : SV_TARGET0;
	float4 normals : SV_TARGET1;
};

float4x4 worldViewProj;

Texture2D picture;
SamplerState pictureSampler;

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	
	output.pos = mul(float4(input.pos, 1.0f), worldViewProj);
	output.tex = input.tex;
	output.normal = input.normal;
	return output;
}

PS_OUT PS( PS_IN input ) : SV_Target
{
	PS_OUT ps_out;
	ps_out.diffuse = picture.Sample(pictureSampler, input.tex);
	ps_out.normals = input.normals
	return 
}
