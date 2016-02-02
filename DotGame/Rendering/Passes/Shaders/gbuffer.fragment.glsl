#version 330 core

struct PS_IN
{
	vec3 normal;
	vec2 textureCoords;
};

in PS_IN vertexData;

layout(location=0) out vec4 fragmentColor;
layout(location=1) out vec4 normals;

uniform sampler2D diffuse;

void main(void) {
	fragmentColor = texture(diffuse, vec2(vertexData.textureCoords.x, 1 - vertexData.textureCoords.y));
	normals = vec4(vertexData.normal, 0);
}