extends Control

func _ready() -> void:
	GameManager.register(Events.TitleScreen, _open_title_screen)
	if GameManager._no_title_screen: visible = false

func _on_button_pressed() -> void:
	visible = false
	GameManager.raise(Events.GameStart)
	GameManager.raise(Events.StartDay)

func _open_title_screen() -> void:
	visible = true
