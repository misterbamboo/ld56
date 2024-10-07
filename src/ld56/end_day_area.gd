extends Area3D

var playerIn = false

func _intput(event:InputEvent)->void:
	if event.is_action_pressed("action"):
		if playerIn:
			GameManager.raise(Events.EndDay)

func bodyEntered(_body:Node3D) -> void:
	pass
	
func bodyExited(_body:Node3D) -> void:
	pass
