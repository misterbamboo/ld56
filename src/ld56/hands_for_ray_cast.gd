extends Node3D

const RAY_LENGTH = 1000

@export var pick_distance: float = 3
@export var cam: Camera3D
@export var selectionDot: Control

var _hover_item: BaseItem = null
var _picked_item: BaseItem = null

var _last_mouse_pressed: bool = false

func _ready() -> void:
	selectionDot.visible = false

func _process(_delta: float) -> void:
	_update_selection_dot()
	_hold_item()
	_move_picked_item()
	
func _hold_item() -> void:
	var current_mouse_pressed: bool = Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT)
	if current_mouse_pressed != _last_mouse_pressed:
		_last_mouse_pressed = current_mouse_pressed
		if current_mouse_pressed:
			_try_pick()
		else:
			_release_item()	
	
func _update_selection_dot() -> void:
	selectionDot.visible = _hover_item != null or _picked_item != null
	
func _move_picked_item() -> void:
	if _picked_item == null:
		return
	
	var forward = cam.global_transform.basis.z.normalized() * 2
	var newPos = global_position - forward
	
	var target_floor_pos = Vector3(newPos.x, 0, newPos.z)
	var hands_floor_pos = Vector3(global_position.x, 0, global_position.z)
	
	var direction_vector = (target_floor_pos - hands_floor_pos).normalized()
	var target_pos = hands_floor_pos + direction_vector + Vector3(0, newPos.y, 0)
	
	var yaw = atan2(direction_vector.x, direction_vector.z)
	
	_picked_item.rb.global_position = target_pos
	_picked_item.rb.rotation = Vector3(_picked_item.rb.global_rotation.x, yaw, _picked_item.rb.global_rotation.z)

func _physics_process(_delta: float) -> void:
	var new_hover_item: BaseItem = null
	
	var space_state = get_world_3d().direct_space_state
	var mousepos = get_viewport().get_mouse_position()

	var origin = cam.project_ray_origin(mousepos)
	var end = origin + cam.project_ray_normal(mousepos) * RAY_LENGTH
	var query = PhysicsRayQueryParameters3D.create(origin, end)
	query.collide_with_areas = true

	var result = space_state.intersect_ray(query)
	if result and result.collider is RigidBody3D:
		var distanceVector: Vector3 = result.position - global_position
		var distance = distanceVector.length()
		if distance <= pick_distance:
			var collider: RigidBody3D = result.collider
			var parent = collider.get_parent()
			if parent is BaseItem:
				new_hover_item = parent
	
	_hover_item = new_hover_item

func _try_pick() -> void:
	if _hover_item == null:
		return
	
	_picked_item = _hover_item
	_picked_item.freeze()

func _release_item() -> void:
	if _picked_item == null:
		return
		
	_picked_item.unfreeze()
	_picked_item = null
