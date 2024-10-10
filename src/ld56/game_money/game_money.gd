extends Control

@onready var label:Label = $Label
@onready var label2:Label = $QuotaDisplay/Label2
@onready var label3:Label = $QuotaDisplay/Label3
@onready var audio:AudioStreamPlayer2D = $AudioStreamPlayer2D
@onready var displayTimer:Timer = $Timer

var opacity = 0.0
var should_hide = true
var _previous_money: float = 0

func _ready() -> void:
	GameManager.register(Events.MoneyReceived, _money_received)
	GameManager.register(Events.GameStart, _show_money)
	modulate = Color(1,1,1,0)

func _process(delta: float) -> void:
	if should_hide:
		if opacity > 0:
			opacity -= delta
			if opacity <= 0: opacity = 0
	else:
		if opacity < 1:
			opacity += delta
			if opacity >= 1: opacity = 1
			
	modulate = Color(1,1,1, opacity)

func _money_received() -> void:
	var current_money = GameManager.get_unchashed_in_money()
	label.text = "%d $" % GameManager.get_money()
	label2.text = "%d $" % current_money
	label3.text = "%d $" % GameManager.quota
	
	if current_money > _previous_money:
		audio.play(0)

	_previous_money = current_money
	
	opacity = 1
	should_hide = false
	displayTimer.start()
	
func _show_money()->void:
	var current_money = GameManager.get_unchashed_in_money()
	label.text = "%d $" % GameManager.get_money()
	label2.text = "%d $" % current_money
	label3.text = "%d $" % GameManager.quota
	
	opacity = 1
	should_hide = false
	displayTimer.start()
	
func display_timeout()->void:
	should_hide = true
