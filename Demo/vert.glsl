#version 330 core

in vec2 uv;
in vec3 pos;

out vec2 fraguv;

void main() {
	fraguv = uv;
    gl_Position = vec4(pos, 1.0); 
}