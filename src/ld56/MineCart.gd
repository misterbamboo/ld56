class_name MineCart extends PathFollow3D

@onready var pushCollider: Area3D = $Area3D
@onready var target: Node3D = $PlayerTarget

var playerInBox = false
var playerRef: Player

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("action"):
		if playerRef != null && !playerRef.is_attached_to_cart:
			playerRef.AttachToCart()
			playerRef.reparent(self)
		#print("click")
	
	if event.is_action_released("action"):
		if playerRef != null && playerRef.is_attached_to_cart:
			playerRef.DetachFromCart()
			playerRef.reparent(find_parent("Master"))
		#print("unclick")
	
func _physics_process(delta: float) -> void:
	if playerRef == null || !playerRef.is_attached_to_cart:
		return
		
	var direction = Input.get_axis("back", "forward")
	
	var speed:float = playerRef.SPEED * playerRef.CART_SPEED_MOD;
	
	if playerRef.is_out_of_breath:
		playerRef.stamina += delta * playerRef.stamina_recharge_rate_multiplier
		if playerRef.stamina > playerRef.total_stamina_in_seconds * playerRef.out_of_breath_minimum_refill_percent:
			playerRef.is_out_of_breath = false
	elif Input.is_action_pressed("run"):
		if playerRef.stamina > 0:
			speed = speed * playerRef.run_speed_modifier
			playerRef.stamina -= delta
			if playerRef.stamina <= 0:
				playerRef.is_out_of_breath = true
				playerRef.PlayOutOfBreathSound()
	elif playerRef.stamina < playerRef.total_stamina_in_seconds:
		playerRef.stamina += delta * playerRef.stamina_recharge_rate_multiplier
		
	if Input.is_action_pressed("run"):
		speed = speed * playerRef.run_speed_modifier
		
	var newprogress = progress + direction * speed
	progress = clamp(newprogress, 0, get_parent().curve.get_baked_length());

func bodyEnter(body: Node3D) -> void:
	playerInBox = true
	playerRef = body
	print("in")
	
func bodyExit(_body: Node3D) -> void:
	print("out")
	playerInBox = false
	playerRef = null
