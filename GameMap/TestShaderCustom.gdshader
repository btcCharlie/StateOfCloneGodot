shader_type spatial;

varying vec4 custom0;

void vertex() {
	custom0 = CUSTOM0;
}

void fragment() {
	ALBEDO = custom0.xyz;
}


