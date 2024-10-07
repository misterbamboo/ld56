extends Control

@export var label: Label

var starting_time_mins: float = 5
var time: float

var last_display: String = ""

var game_started: bool = false

func _ready() -> void:
	GameManager.register(Events.GameStart, _start_timer)
	GameManager.register(Events.GameOver, func(): game_started = false)

func _start_timer():
	time = starting_time_mins * 60
	game_started = true

func _process(delta: float) -> void:
	time = clamp(time - delta, 0, 999999999)
	_display_time()
	_trigger_timeout()

func _display_time() -> void:
	@warning_ignore("narrowing_conversion") 
	var mins:int = time / 60.0 
	@warning_ignore("narrowing_conversion") 
	var timeInt: int = time
	var secs: int = timeInt % 60
	
	var display = _get_display(mins, secs)
	if display != last_display:
		last_display = display
		label.text = display

func _trigger_timeout() -> void:
	if game_started and time <= 0:
		GameManager.raise(Events.GameTimeout)

func _get_display(mins: int, secs: int) -> String:
	var display = ""
	if mins < 10:
		display += "0"
	display += var_to_str(mins)
	display += ":"
	
	if secs < 10:
		display += "0"
	display += var_to_str(secs)
	return display
