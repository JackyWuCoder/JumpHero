// taken from https://github.com/godotengine/godot-demo-projects/blob/3.5-9e68af3/2d/screen_space_shaders/shaders/blur.shader

shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform float amount: hint_range(0.0, 5.0);

void fragment() {
	COLOR.rgb = textureLod(SCREEN_TEXTURE, SCREEN_UV, amount).rgb;
}