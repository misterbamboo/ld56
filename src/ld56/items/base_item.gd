class_name BaseItem extends Node3D

@export var rb: RigidBody3D

func _ready() -> void:
	pass # Replace with function body.

func _process(_delta: float) -> void:
	pass
	
func freeze() -> void:
	rb.rotation = Vector3.ZERO
	rb.freeze = true
	
func unfreeze() -> void:
	rb.freeze = false
