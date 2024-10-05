class_name Player extends CharacterBody3D

const SPEED = 5.0
const JUMP_VELOCITY = 4.5

@export var sensitivity = 0.001

@onready var neck := $Neck
@onready var cam := $Neck/Camera3D

var CameraRotation = Vector2(0,0)

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	elif event.is_action_pressed("ui_cancel"):
		if(Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE):
			get_tree().quit()
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		
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
	if direction:
		velocity.x = direction.x * SPEED
		velocity.z = direction.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.z = move_toward(velocity.z, 0, SPEED)

	move_and_slide()
