class_name Player extends CharacterBody3D

#const SPEED = 4.0
#const JUMP_VELOCITY = 4.5
#const CART_SPEED_MOD = 0.008
#const TIME_TO_SCARE_SECONDS = 0.5
#
#@export var sensitivity = 0.001
#@export var run_speed_modifier = 1.5;

#@onready var neck := $Neck
#@onready var cam := $Neck/Camera3D
#@onready var footstep := $Footsteps
#@onready var first_flashlight_label = $CanvasLayer/Control/FirstFlashlight
#@onready var actionIcon = $CanvasLayer/Control/ActionIcon
#@onready var actionLabel = $CanvasLayer/Control/ActionLabel
#@onready var hud = $CanvasLayer
#@onready var outsideAudio:AudioStreamPlayer3D = $AudioOutsideAmbiance
#@onready var heavyAudio:AudioStreamPlayer3D = $AudioHeavyAmbiance
#@onready var chaseAudio:AudioStreamPlayer3D = $AudioChase
#@onready var outOfBreathAudio: AudioStreamPlayer3D = $AudioOutOfBreath

#var CameraRotation = Vector2(90, 0)
#var is_attached_to_cart = false
#
#var can_move:= false
#var is_inside := false
#var scared := false
#
#var distance_between_footsteps = 150
#var distance_until_next_footsetp = 150
#
#var total_stamina_in_seconds := 5.0
#var stamina := total_stamina_in_seconds
#var stamina_recharge_rate_multiplier := 1.2
#var out_of_breath_minimum_refill_percent = 0.75
#var is_out_of_breath := false
#
#var action: String = ""

#func _ready() -> void:
	#print_rich("[color=green] playerReady![/color]")
	#GameManager.register(Events.GameStart, _on_game_start)
	#GameManager.register(Events.GameOver, _uncapture_mouse)
	#GameManager.register(Events.EndDay, _uncapture_mouse)
	#GameManager.register(Events.HitPlayer, _on_receive_hit)
	#GameManager.register(Events.StartChase, _on_start_chase)
	#GameManager.register(Events.StopChase, _on_stop_chase)
	#GameManager.register_player(self)

#func _on_game_start() -> void:
	#can_move = true
	#display_hud()

#func _on_start_chase() -> void:
	#scared = true
	##heavyAudio.stop()
	
#func _on_stop_chase() -> void:
	#scared = false
	##heavyAudio.play(0)

#func _set_action(label:String) -> void:
	#action = label

#func _process(delta: float) -> void:
	#if scared:
		#if chaseAudio.volume_db < 0:
			#chaseAudio.volume_db += 80 * (delta / TIME_TO_SCARE_SECONDS)
			#if chaseAudio.volume_db > 0: chaseAudio.volume_db = 0
		#
		#if cam.fov < 110:
			#cam.fov += 20 * (delta / TIME_TO_SCARE_SECONDS)
			#if cam.fov > 110: cam.fov = 110
	#else:
		#if chaseAudio.volume_db > -80:
			#chaseAudio.volume_db -= 80 * (delta / TIME_TO_SCARE_SECONDS)
			#if chaseAudio.volume_db < -80: chaseAudio.volume_db = -80
		#
		#if cam.fov > 90:
			#cam.fov -= 20 * (delta / TIME_TO_SCARE_SECONDS)
			#if cam.fov < 90: cam.fov = 90
			#
	#if Input.is_action_pressed("action") || action == "" :
		#actionIcon.visible = false
		#actionLabel.visible = false
	#else:
		#actionIcon.visible = true
		#actionLabel.text = action
		#actionLabel.visible = true

#func _uncapture_mouse() -> void:
	#if chaseAudio.playing: chaseAudio.stop()
	#if heavyAudio.playing: heavyAudio.stop()
	#if outsideAudio.playing: outsideAudio.stop()
	#
	#hide_hud()
	#action = "" # stop displaying action by setting it to empty string
	#can_move = false
	#Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
#func display_hud() ->void:
	#hud.visible = true
	#
#func hide_hud() -> void:
	#hud.visible = false
	
#func _on_receive_hit() -> void:
	#GameManager.player_die()

#func _unhandled_input(event: InputEvent) -> void:
	#if event.is_action_pressed("ui_cancel"):
		#if(Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE):
			#get_tree().quit()
		#Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		#
	#elif event.is_action_pressed("action"):
		#Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		#
	#if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		#if event is InputEventMouseMotion:
			#CameraLook(event.relative * sensitivity)

#func CameraLook (movement: Vector2):
	#CameraRotation += movement
	#CameraRotation.y = clamp(CameraRotation.y, -1.5, 1.2)
	#transform.basis = Basis()
	#cam.basis = Basis()
	#rotate_object_local(Vector3(0,1,0), -CameraRotation.x)
	#cam.rotate_object_local(Vector3(1,0,0), -CameraRotation.y)
			
#func _physics_process(delta: float) -> void:
	#if !can_move: return
	#if is_attached_to_cart: return
	#
	## Add the gravity.
	#if not is_on_floor():
		#velocity += get_gravity() * delta
#
	## Handle jump.
	#if Input.is_action_just_pressed("jump") and is_on_floor():
		#velocity.y = JUMP_VELOCITY
#
	## Get the input direction and handle the movement/deceleration.
	## As good practice, you should replace UI actions with custom gameplay actions.
	#var input_dir := Input.get_vector("left", "right", "forward", "back")
	#var direction := (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	#
	#var speed:float = SPEED;
	#
	#if is_out_of_breath:
		#stamina += delta * stamina_recharge_rate_multiplier
		#if stamina > total_stamina_in_seconds * out_of_breath_minimum_refill_percent:
			#is_out_of_breath = false
	#elif Input.is_action_pressed("run"):
		#if stamina > 0:
			#speed = SPEED * run_speed_modifier
			#stamina -= delta
			#if stamina <= 0:
				#is_out_of_breath = true
				#PlayOutOfBreathSound()
	#elif stamina < total_stamina_in_seconds:
		#stamina += delta * stamina_recharge_rate_multiplier
	#
	#if direction:
		#velocity.x = direction.x * speed
		#velocity.z = direction.z * speed
	#else:
		#velocity.x = move_toward(velocity.x, 0, speed)
		#velocity.z = move_toward(velocity.z, 0, speed)
		#
	#distance_until_next_footsetp -= velocity.length()
	#if(distance_until_next_footsetp < 0):
		#distance_until_next_footsetp = distance_between_footsteps
		#footstep.play(0)
	#
	#move_and_slide()

#func AttachToCart() -> void:
	#is_attached_to_cart = true
#
#func DetachFromCart() -> void:
	#is_attached_to_cart = false

#func PlayInsideSound(_body: Node3D) -> void:
	#print("inside!")
	#is_inside = true
	#if(heavyAudio.playing): return
	#heavyAudio.play(0)
	#outsideAudio.stop()
	
#func PlayOutsideSound(_body: Node3D) -> void:
	#print("outside!")
	#is_inside = false
	#if(outsideAudio.playing): return
	#outsideAudio.play(0)
	#heavyAudio.stop()

#func PlayOutOfBreathSound() -> void:
	#outOfBreathAudio.play(0)
	
	
