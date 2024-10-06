extends AnimationPlayer

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	GameManager.register("gameover", _start_animation)
	_hide_gameoverscreen()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass

func _start_animation() -> void:
	_unhide_gameoverscreen()
	seek(0, true)
	play("gameover_move_up")

func _on_button_pressed() -> void:
	GameManager.reset_game()

func _hide_gameoverscreen() -> void:
	get_parent().hide()
	
func _unhide_gameoverscreen() -> void:	
	get_parent().visible = true
