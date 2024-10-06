extends Control

@export var label: Label
@export var audio: AudioStreamPlayer2D

var _previous_money: float = 0

func _ready() -> void:
	GameManager.register("moneyreceived", _money_received)
	GameManager.register("gamestart", _money_received)

func _process(delta: float) -> void:
	pass

func _money_received() -> void:
	var current_money = GameManager.get_money()
	label.text = "" + var_to_str(current_money) + "$"
	
	if current_money > _previous_money:
		audio.play(0)

	_previous_money = current_money
