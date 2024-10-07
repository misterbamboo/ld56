class_name Flashlight extends Node3D

const AMBIENT_STRENGHT_ON = 0.15
const AMBIENT_STRENGHT_OFF = 0.05

@onready var flashLight := $Flashlight
@onready var ambientLight := $AmbientLight

@onready var audio_on :=$audio_flash_on
@onready var audio_off :=$audio_flash_off

var flashlight_on := false
var first_flashlight = true
var battery_life_in_seconds_max: float = 120
var battery_life_in_seconds: float = 120

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
		audio_on.play(0)
	else:
		turn_off()
		audio_off.play(0)

func turn_on() -> void:
	if first_flashlight:
		first_flashlight = false
		find_parent("Player").get_node("CanvasLayer/Control/FirstFlashlight").visible = false
		
	flashlight_on = true
	flashLight.visible = true
	ambientLight.light_energy = AMBIENT_STRENGHT_ON

func turn_off() -> void:
	flashlight_on = false
	flashLight.visible = false
	ambientLight.light_energy = AMBIENT_STRENGHT_OFF
