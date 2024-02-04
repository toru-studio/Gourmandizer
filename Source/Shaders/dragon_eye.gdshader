shader_type canvas_item;

uniform vec2 motion = vec2(0.0);
uniform float scale = 1.0;
uniform vec2 offset = vec2(0.0);
uniform float offset_scale = 0.0;

void vertex(){
    float oscillation = sin(TIME);
    UV = (VERTEX + offset + oscillation * offset_scale * motion) * TEXTURE_PIXEL_SIZE * (1.0/scale);
}