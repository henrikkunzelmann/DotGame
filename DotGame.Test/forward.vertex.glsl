#version 330 core

in vec3 in_position;
in vec2 in_texCoord;

layout(std140) uniform global 
{
	mat4 modelViewProjection;
};

struct PS_IN
{
	vec4 position;
	vec2 texCoord;
	vec3 worldPosition;
};

out PS_IN pixelShaderData;

void main(void) {
    
    pixelShaderData.position = vec4(in_position, 1.0f) * modelViewProjection;
    pixelShaderData.worldPosition = in_position;
	pixelShaderData.texCoord = in_texCoord;
	
	gl_Position = pixelShaderData.position;
}