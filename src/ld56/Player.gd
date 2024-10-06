class_name Player extends CharacterBody3D

const SPEED = 5.0
const JUMP_VELOCITY = 4.5
const CART_SPEED_MOD = 0.01

@export var sensitivity = 0.001
@export var run_speed_modifier = 1.5;

@onready var neck := $Neck
@onready var cam := $Neck/Camera3D
@onready var footstep := $Footsteps

@onready var outsideAudio:AudioStreamPlayer3D = $AudioOutsideAmbiance
@onready var heavyAudio:AudioStreamPlayer3D = $AudioHeavyAmbiance

var CameraRotation = Vector2(90, 0)
var is_attached_to_cart = false

var can_move: bool = false

var distance_between_footsteps = 120
var distance_until_next_footsetp = 120

func _ready() -> void:
	GameManager.register("gamestart", func(): can_move = true)
	GameManager.register("gameover", _uncapture_mouse)
	GameManager.register("hitplayer", _on_receive_hit)
	GameManager.register_player(self)

func _uncapture_mouse() -> void:
	can_move = false
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
func _on_receive_hit() -> void:
	GameManager.player_die()

func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if(Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE):
			get_tree().quit()
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		
	elif event.is_action_pressed("action"):
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		
	if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		if event is InputEventMouseMotion:
			CameraLook(event.relative * sensitivity)

func CameraLook (movement: Vector2):
	CameraRotation += movement
	CameraRotation.y = clamp(CameraRotation.y, -1.5, 1.2)
	transform.basis = Basis()
	cam.basis = Basis()
	rotate_object_local(Vector3(0,1,0), -CameraRotation.x)
	cam.rotate_object_local(Vector3(1,0,0), -CameraRotation.y)
			
func _physics_process(delta: float) -> void:
	if !can_move: return
	if is_attached_to_cart: return
	
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var input_dir := Input.get_vector("left", "right", "forward", "back")
	var direction := (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	var speed:float = SPEED;
	
	if Input.is_action_pressed("run"):
		speed = SPEED * run_speed_modifier
		
	if direction:
		velocity.x = direction.x * speed
		velocity.z = direction.z * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
		velocity.z = move_toward(velocity.z, 0, speed)
		
	distance_until_next_footsetp -= velocity.length()
	if(distance_until_next_footsetp < 0):
		distance_until_next_footsetp = distance_between_footsteps
		footstep.play(0)
	
	move_and_slide()

func AttachToCart() -> void:
	is_attached_to_cart = true

func DetachFromCart() -> void:
	is_attached_to_cart = false

func PlayInsideSound(body: Node3D) -> void:
	if(body.name != "Player"): return
	if(heavyAudio.playing): return
	heavyAudio.play(0)
	outsideAudio.stop()
	
func PlayOutsideSound(body: Node3D) -> void:
	if(body.name != "Player"): return
	if(outsideAudio.playing): return
	outsideAudio.play(0)
	heavyAudio.stop()
