#version 330 core

layout(location = 0) in vec4 in_position;
layout(location = 1) in vec3 in_normal;
layout(location = 2) in vec2 in_texcoord;


layout(std140) uniform global 
{
	mat4 modelViewProjection;
};


struct PS_IN
{
	vec3 normal;
	vec2 textureCoords;
};

out PS_IN vertexData;

void main(void) {

	vec4 finalPosition = in_position * modelViewProjection;	
	gl_Position = finalPosition;
	
	vertexData.normal = in_normal;
	vertexData.textureCoords = in_texcoord;
}