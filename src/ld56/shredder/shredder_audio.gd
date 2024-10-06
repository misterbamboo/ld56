extends AudioStreamPlayer3D

func _ready() -> void:
	GameManager.register("gamestart", func(): play(0))
	GameManager.register("titlescreen", func(): stop())
