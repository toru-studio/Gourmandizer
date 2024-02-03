shader_type canvas_item;

uniform vec2 motion = vec2(0.0);
uniform float scale = 1.0;
uniform vec2 offset = vec2(0.0);
uniform float offset_scale = 0.0;

void vertex(){
    float oscillation = sin(TIME);
    UV = (VERTEX + offset + oscillation * offset_scale * motion) * TEXTURE_PIXEL_SIZE * (1.0/scale);
}

void fragment() {
    // Sample the texture
    vec4 texture_color = texture(TEXTURE, UV);

    // Color shift using a sine function on the red channel
    float red_shift = 0.09 * sin(TIME);
    float green_shift = 0.06 * sin(TIME);
    float blue_shift = 0.03 * sin(TIME);
    texture_color.r += red_shift;
    texture_color.g += green_shift;
    texture_color.b += blue_shift;

    // Output the modified color
    COLOR = texture_color;
}
