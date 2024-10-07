class_name Skitter extends CharacterBody3D

@export var speed: float
@export var acceleration: float
@export var patrolNodes: Array

@onready var nav: NavigationAgent3D = $NavigationAgent3D
@onready var raycast: RayCast3D = $RayCast3D
@onready var skitterAudio: AudioStreamPlayer3D = $AudioStreamPlayer3D
@onready var animator: AnimationPlayer = $AnimationPlayer

const SKITTER_AUDIO_BOOST := 10

var player_in_range := false
var see_player := false
var skitter := false
var going := false

var skitterGoTimeInSecondsLow: float = 0.5
var skitterGoTimeInSecondsUp: float = 2
var skitterStopTimeInSecondsLow: float = 0.5
var skitterStopTimeInSecondsUp: float = 1.5

var skittergoTimer = skitterGoTimeInSecondsLow
var skitterstopTimer = skitterStopTimeInSecondsLow

func _physics_process(delta:float)->void:
	var target = to_local(GameManager.get_player().global_position) - Vector3(0,1,0)
	
	if (global_position-GameManager.get_player().global_position).length() < 2:
		GameManager.player_die()
	raycast.target_position = target
	var collider = raycast.get_collider()
	see_player = collider != null && collider.name == "Player"
	var look_pos = global_position - velocity.normalized()
	
	if skitter:
		if going:
			skittergoTimer -= delta
			if skittergoTimer <= 0:
				skitterAudio.stop()
				animator.stop()
				going = false
				skitterstopTimer = randf_range(skitterStopTimeInSecondsLow, skitterStopTimeInSecondsUp)
			move_and_slide()
		else:
			skitterstopTimer -= delta
			if skitterstopTimer <= 0:
				skitterAudio.play(0)
				animator.play("skittering_anim")
				going = true
				skittergoTimer = randf_range(skitterGoTimeInSecondsLow, skitterGoTimeInSecondsUp)
	else:
		if(!skitterAudio.playing):
			skitterAudio.play(0)
		if(!animator.is_playing()):
			animator.play("skittering_anim")
		move_and_slide()
	
	if velocity.length() != 0:
		look_at(Vector3(look_pos.x, global_position.y, look_pos.z))
		

	
func bodyEnter(_body: Node3D)->void:
	player_in_range = true
	print("body enter")
	pass

func bodyExit(_body: Node3D)->void:
	player_in_range = false
	print("body exit")
	pass
