shader_type spatial;

render_mode specular_toon;

uniform sampler2D noise;
uniform sampler2D normalmap;
uniform float height_scale = 0.2;

varying vec2 pos;

float wave(vec2 position) {
	position += texture(noise, position / 10.0).x * 2.0 - 1.0;
	vec2 wv = 1.0 - abs(sin(position));
	return pow(1.0 - pow(wv.x * wv.y, 0.65), 4.0);
}

float height(vec2 position, float time) {
	vec2 t1 = vec2(position.x + time, position.y - time);
	vec2 t2 = vec2(position.x - time, position.y + time);
	float d = wave((position + time) * 0.4) * 0.3;
	d += wave((position - time) * 0.3) * 0.3;
	d += wave((t1) * 0.5) * 0.2;
	d += wave((t2) * 0.6) * 0.2;
	return d;
	vec2 offset = 0.01 * cos(position + time);
	return texture(noise, (position / 10.0) - offset).x * height_scale;
}

void vertex() {
	pos = VERTEX.xz;
	float k = height(pos, TIME);
	VERTEX.y = k;
	NORMAL = normalize(vec3(k - height(pos + vec2(0.1, 0.0), TIME), 0.1, k - height(pos + vec2(0.0, 0.1), TIME)));
}

void fragment() {
	float fresnel = sqrt(1.0 - dot(NORMAL, VIEW));
	RIM = 0.2;
	METALLIC = 0.0;
	ROUGHNESS = 0.01 * (1.0 - fresnel);
	ALBEDO = vec3(0.01, 0.03, 0.05) + (0.1 * fresnel);
}
