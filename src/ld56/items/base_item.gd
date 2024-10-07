class_name BaseItem extends Node3D

@export var rb: RigidBody3D
var grinding: bool = false

func _ready() -> void:
	rb.freeze_mode = RigidBody3D.FREEZE_MODE_KINEMATIC

func _process(_delta: float) -> void:
	pass
	
func freeze() -> void:
	rb.rotation = Vector3.ZERO
	rb.freeze = true
	
func unfreeze() -> void:
	rb.freeze = false

func grind() -> void:
	grinding = true

func destroy() -> void:
	GameManager.give_money(10)
	queue_free()
