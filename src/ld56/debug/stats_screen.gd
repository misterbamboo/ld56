extends Node

@export var label:Label

func _ready() -> void:
	StatsScreen.label = label
	pass

func distance(text:String) -> void:
	if not label:
		return
	
	label.text = "stats:\n" + text
