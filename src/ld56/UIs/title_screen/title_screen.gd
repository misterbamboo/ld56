extends Control

func _ready() -> void:
	GameManager.register("titlescreen", _open_title_screen)

func _on_button_pressed() -> void:
	visible = false
	GameManager.raise("gamestart")
	GameManager.raise("DayStarted")

func _open_title_screen() -> void:
	visible = true
