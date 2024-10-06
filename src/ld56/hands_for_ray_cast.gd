extends Node3D

const RAY_LENGTH = 1000

@export var cam: Camera3D

var _pickedItem: BaseItem = null

func _ready() -> void:
	pass # Replace with function body.

func _process(delta: float) -> void:
	if _pickedItem:
		var node: Node3D = _pickedItem
		
		var forward = cam.global_transform.basis.z.normalized() * 2
		var newPos = global_position - forward
		
		var target_floor_pos = Vector3(newPos.x, 0, newPos.z)
		var hands_floor_pos = Vector3(global_position.x, 0, global_position.z)
		
		var direction_vector = (target_floor_pos - hands_floor_pos).normalized()
		var target_pos = hands_floor_pos + direction_vector + Vector3(0, newPos.y + 1, 0)
		
		var yaw = atan2(direction_vector.x, direction_vector.z)
		
		node.global_position = target_pos
		node.rotation = Vector3(node.global_rotation.x, yaw, node.global_rotation.z)

func _physics_process(delta: float) -> void:
	var space_state = get_world_3d().direct_space_state
	var mousepos = get_viewport().get_mouse_position()

	var origin = cam.project_ray_origin(mousepos)
	var end = origin + cam.project_ray_normal(mousepos) * RAY_LENGTH
	var query = PhysicsRayQueryParameters3D.create(origin, end)
	query.collide_with_areas = true

	var result = space_state.intersect_ray(query)
	if result and result.collider is RigidBody3D:
		var collider: RigidBody3D = result.collider
		var parent = collider.get_parent()
		if parent is BaseItem:
			var baseItem: BaseItem = parent
			pick(baseItem)
			
func pick(baseItem: BaseItem) -> void:
	_pickedItem = baseItem
	_pickedItem.freeze()
