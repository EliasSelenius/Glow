#version 330 core

in vec2 fraguv;
out vec4 outColor;

uniform sampler2D mainTexture;

void main() {
    outColor = texture(mainTexture, fraguv); 
}