extends AudioStreamPlayer3D

func _ready() -> void:
	GameManager.register(Events.GameStart, func(): play(0))
	GameManager.register(Events.TitleScreen, func(): stop())
