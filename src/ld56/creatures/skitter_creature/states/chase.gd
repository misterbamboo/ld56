extends FSMState

# Executes after the state is entered.
func _on_enter(actor: Node, _blackboard: Blackboard) -> void:
	actor = actor as Skitter
	actor.skitter = false
	GameManager.raise("startchase")
	
# Executes every _process call, if the state is active.
func _on_update(_delta: float, actor: Node, _blackboard: Blackboard) -> void:
	actor = actor as Skitter
	var direction = Vector3()
	actor.nav.target_position = GameManager.get_player().global_position
	direction = actor.nav.get_next_path_position() - actor.global_position
	direction = direction.normalized()
	
	actor.velocity = actor.velocity.lerp(direction * actor.speed, actor.acceleration * _delta)

# Executes before the state is exited.
func _on_exit(_actor: Node, _blackboard: Blackboard) -> void:
	pass


# Add custom configuration warnings
# Note: Can be deleted if you don't want to define your own warnings.
func _get_configuration_warnings() -> PackedStringArray:
	var warnings: Array = []

	warnings.append_array(super._get_configuration_warnings())

	# Add your own warnings to the array here

	return warnings
