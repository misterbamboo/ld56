class_name Flashlight extends Node3D

const AMBIENT_STRENGHT_ON = 0.3
const AMBIENT_STRENGHT_OFF = 0.05

@onready var flashLight := $Flashlight
@onready var ambientLight := $AmbientLight

var flashlight_on := false
var battery_life_in_seconds_max = 120
var battery_life_in_seconds = 120

func _input(_event: InputEvent)->void:
	if Input.is_action_just_pressed("toggle_flashlight"):
		toggleFlashlight()
		
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if flashlight_on && battery_life_in_seconds > 0:
		battery_life_in_seconds -= delta
		
		if battery_life_in_seconds <= 0:
			battery_life_in_seconds = 0
			turn_off()
	
func toggleFlashlight() -> void:
	if(battery_life_in_seconds <= 0):
		pass
	
	if !flashlight_on:
		turn_on()
	else:
		turn_off()

func turn_on() -> void:
	flashlight_on = true
	flashLight.visible = true
	ambientLight.light_energy = AMBIENT_STRENGHT_ON

func turn_off() -> void:
	flashlight_on = false
	flashLight.visible = false
	ambientLight.light_energy = AMBIENT_STRENGHT_OFF
