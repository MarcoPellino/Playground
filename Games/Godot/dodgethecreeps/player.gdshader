shader_type canvas_item;

uniform bool immune = false;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 tex = texture(TEXTURE, UV);
    if (immune) {
        tex.rgb = mix(tex.rgb, vec3(0.4, 0.7, 1.0), 0.5); // sfuma verso azzurro
    }
    COLOR = tex;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
