extends TextureRect

var t: float = 0
var degrees_per_second: float = 180

func _process(delta: float) -> void:
	t += delta
	rotation_degrees = t * degrees_per_second
