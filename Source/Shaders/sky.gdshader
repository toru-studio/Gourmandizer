shader_type canvas_item;

uniform vec2 motion = vec2(0.0);
uniform float scale = 1.0;
uniform vec2 offset = vec2(0.0);

void vertex() {
    UV = (VERTEX + offset + TIME * motion) * TEXTURE_PIXEL_SIZE * (1.0/scale);
}


// Function to convert RGB to HSV
vec3 rgb2hsv(vec3 c) {
    float K = 0.0;
    if (c.g < c.b) {
        K = c.g;
    } else {
        K = c.b;
    }

    float chroma = 0.0;
    if (c.r > c.g && c.r > c.b) {
        chroma = c.r - min(c.g, c.b);
    } else if (c.g > c.b) {
        chroma = c.g - c.b;
    } else {
        chroma = c.b - c.g;
    }

    float hue = 0.0;
    if (chroma != 0.0) {
        if (c.r == K) {
            hue = mod((c.g - c.b) / chroma, 6.0);
        } else if (c.g == K) {
            hue = 2.0 + (c.b - c.r) / chroma;
        } else {
            hue = 4.0 + (c.r - c.g) / chroma;
        }
    }

    hue = hue * 60.0;

    if (hue < 0.0) {
        hue += 360.0;
    }

    float saturation = (K == 0.0) ? 0.0 : (chroma / K);
    float value = K;

    return vec3(hue / 360.0, saturation, value);
}

// Function to convert HSV to RGB
vec3 hsv2rgb(vec3 c) {
    float chroma = c.z * c.y;
    float hue = c.x * 360.0 / 60.0;
    float x = chroma * (1.0 - abs(mod(hue, 2.0) - 1.0));

    vec3 rgb = vec3(0.0);
    if (hue >= 0.0 && hue < 1.0) {
        rgb = vec3(chroma, x, 0.0);
    } else if (hue >= 1.0 && hue < 2.0) {
        rgb = vec3(x, chroma, 0.0);
    } else if (hue >= 2.0 && hue < 3.0) {
        rgb = vec3(0.0, chroma, x);
    } else if (hue >= 3.0 && hue < 4.0) {
        rgb = vec3(0.0, x, chroma);
    } else if (hue >= 4.0 && hue < 5.0) {
        rgb = vec3(x, 0.0, chroma);
    } else {
        rgb = vec3(chroma, 0.0, x);
    }

    float m = c.z - chroma;
    rgb += vec3(m, m, m);

    return rgb;
}

void fragment() {
    // Sample the texture
    vec4 texture_color = texture(TEXTURE, UV);

    // Convert RGB to HSV
    vec3 hsv = rgb2hsv(texture_color.rgb);

    // Shift the hue component based on time
    hsv.x += 0.05 * TIME;

    // Wrap the hue value within [0, 1)
    hsv.x = mod(hsv.x, 1.0);

    // Convert back to RGB
    vec3 shifted_rgb = hsv2rgb(hsv);

    // Output the modified color
    COLOR = vec4(shifted_rgb, texture_color.a);
}
