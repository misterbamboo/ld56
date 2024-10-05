extends Node

func _ready() -> void:
	var parent_node = get_parent()
	var default_pos = Vector3(0, parent_node.position.y, 0)
	parent_node.position = default_pos
