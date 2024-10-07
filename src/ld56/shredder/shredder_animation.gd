extends AnimationPlayer

func _ready() -> void:
	play("grind")
	
func _on_animation_finished(_anim_name: StringName) -> void:
	play("grind")
