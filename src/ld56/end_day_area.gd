extends Area3D

var playerIn = false

func _unhandled_input(event:InputEvent)->void:
	if event.is_action_pressed("action"):
		if playerIn:
			GameManager.raise(Events.EndDay)
			# set the input as handled cause sinon le click est registered par
			# le player dans son _unhandled_input et remet la souris en captured 
			get_viewport().set_input_as_handled()
func bodyEntered(_body:Node3D) -> void:
	playerIn = true
	GameManager.set_action("Leave")
	
func bodyExited(_body:Node3D) -> void:
	playerIn = false
	GameManager.set_action("")
