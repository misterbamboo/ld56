extends Control

func _ready() -> void:
	GameManager.register(Events.TitleScreen, _open_title_screen)

func _on_button_pressed() -> void:
	visible = false
	GameManager.raise(Events.GameStart)
	GameManager.raise(Events.StartDay)

func _open_title_screen() -> void:
	visible = true
