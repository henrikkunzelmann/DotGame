struct VS_IN
{
	float3 pos : POSITION; 
};

struct PS_IN
{
	float4 pos : SV_POSITION;  
	float3 worldPos : TEXCOORD0;
};

float4x4 worldViewProj;

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	
	output.pos = mul(float4(input.pos, 1.0f), worldViewProj);
	output.worldPos = input.pos;
	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	return float4(abs(input.worldPos), 1.0f);
}
