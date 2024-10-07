extends Control

func _on_button_pressed() -> void:
	visible = false
	GameManager.raise(Events.GameStart)
	GameManager.raise(Events.StartDay)
