extends Node

@export var label:Label

const LINE_DISTANCE: int = 0
const LINE2: int = 1
const LINE3: int = 2
const LINE4: int = 3
const LINE5: int = 4
var lines_names: Array[String] = ["distance: ", "retrieve_path: ", "", "", "", ""]

var lines: Array[String] = ["", "", "", "", "", ""]

func _ready() -> void:
	StatsScreen.label = label
	pass

func update_line(line_index: int, text:String) -> void:
	if not label:
		return
	
	lines[line_index] = lines_names[line_index] + text
	
	_print_lines()
	
func _print_lines() -> void:
	var concat: String = ""
	for line in lines:
		concat += line + "\n"
		
	label.text = concat
