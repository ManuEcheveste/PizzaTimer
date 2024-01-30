shader_type canvas_item;

uniform sampler2D fill :repeat_enable;
uniform vec2 direction = vec2(1.0,0);
uniform float speedScale = 0.15;

void fragment() {
	vec2 move = direction * TIME * speedScale;
	COLOR = texture(TEXTURE, UV + move);
}
