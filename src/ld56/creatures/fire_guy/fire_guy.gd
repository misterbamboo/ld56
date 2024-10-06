class_name FireGuy extends CharacterBody3D

@export var speed: float
@export var acceleration:float
@export var patrolNodes: Array

@onready var nav: NavigationAgent3D = $NavigationAgent3D
@onready var raycast: RayCast3D = $RayCast3D

var player_in_range := false
var see_player := false

func _physics_process(delta:float)->void:
	var target = to_local(GameManager.get_player().global_position) - Vector3(0,1,0)
	raycast.target_position = target
	var collider = raycast.get_collider()
	see_player = collider != null && collider.name == "Player"
	print(see_player)
	var look_pos = global_position - velocity.normalized()
	move_and_slide()
	look_at(Vector3(look_pos.x, global_position.y, look_pos.z))
	
func bodyEnter(body: Node3D)->void:
	player_in_range = true
	print("body enter")
	pass

func bodyExit(body: Node3D)->void:
	player_in_range = false
	print("body exit")
	pass
