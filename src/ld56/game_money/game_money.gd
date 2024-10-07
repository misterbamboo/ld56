extends Control

@onready var label:Label = $Label
@onready var label2:Label = $Label2
@onready var audio:AudioStreamPlayer2D = $AudioStreamPlayer2D

var _previous_money: float = 0

func _ready() -> void:
	GameManager.register(Events.MoneyReceived, _money_received)
	GameManager.register(Events.GameStart, _money_received)

func _process(_delta: float) -> void:
	pass

func _money_received() -> void:
	var current_money = GameManager.get_unchashed_in_money()
	label.text = "%d $" % GameManager.get_money()
	label2.text = "%d $" % current_money
	
	if current_money > _previous_money:
		audio.play(0)

	_previous_money = current_money
