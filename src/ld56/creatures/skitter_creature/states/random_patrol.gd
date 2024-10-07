extends FSMState

var patrolingToNode: Vector3

# Executes after the state is entered.
func _on_enter(actor: Node, _blackboard: Blackboard) -> void:
	actor = actor as Skitter
	actor.skitter = true
	patrolingToNode = actor.patrolNodes.pick_random()
	
# Executes every _process call, if the state is active.
func _on_update(_delta: float, actor: Node, _blackboard: Blackboard) -> void:
	actor = actor as Skitter
	
	var direction = Vector3()
	actor.nav.target_position = patrolingToNode
	direction = actor.nav.get_next_path_position() - actor.global_position
	direction = direction.normalized()
	
	actor.velocity = actor.velocity.lerp(direction * actor.speed, actor.acceleration * _delta)

	pass

# Executes before the state is exited.
func _on_exit(_actor: Node, _blackboard: Blackboard) -> void:
	pass
