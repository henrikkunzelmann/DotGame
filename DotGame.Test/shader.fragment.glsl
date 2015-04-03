#version 330 core

struct PS_IN
{
	vec4 position;
	vec2 texCoord;
	vec3 worldPosition;
};

in PS_IN pixelShaderData;

out vec4 fragmentColor;

uniform sampler2D picture;

void main(void) {
	//fragmentColor = vec4(abs(pixelShaderData.position.xyz), 1.0f);
	fragmentColor = texture(picture, vec2(pixelShaderData.texCoord.x, 1f - pixelShaderData.texCoord.y));
}