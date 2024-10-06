extends Node

@export var label: Label

var toggle_stats: bool = false

const LINE1: int = 0
const LINE2: int = 1
const LINE3: int = 2
const LINE4: int = 3
const LINE5: int = 4
var lines_names: Array[String] = ["distance: ", "player_in_range: ", "retrieve_path: ", "t : ", "", ""]

var lines: Array[String] = ["", "", "", "", "", ""]

func _ready() -> void:
	if label:
		StatsScreen.label = label
		StatsScreen.label.visible = false

var _last_is_key_pressed: bool = false
func _process(_delta: float) -> void:
	var is_key_pressed = Input.is_key_pressed(KEY_F1)
	if is_key_pressed and _last_is_key_pressed != is_key_pressed :
		toggle_stats = !toggle_stats
		StatsScreen.label.visible = toggle_stats
		
	_last_is_key_pressed = is_key_pressed

func update_line(line_index: int, text:String) -> void:
	if not label:
		return
	
	lines[line_index] = lines_names[line_index] + text
	
	_print_lines()
	
func _print_lines() -> void:
	var concat: String = ""
	for i in range(0, len(lines)):
		var line = lines[i]
		concat += line + "\n"
		
	label.text = concat
