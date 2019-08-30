#version 330 core

in vec2 fraguv;
out vec4 outColor;
void main() {
    outColor = vec4(fraguv, 0.0, 1.0); 
}