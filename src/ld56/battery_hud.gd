extends Control

@export var textures: Array = []

var lights: Flashlight
@onready var batteryIcon:TextureRect = $TextureRect

var battery_icon_flash = false

var _can_display: bool = false

func _ready() -> void:
	lights = find_parent("Player").get_node("Neck/Camera3D/Lights")
	GameManager.register(Events.GameStart, func(): _can_display = true )
	GameManager.register(Events.GameOver, func(): _can_display = false)

# Called by signal from timer
func checkBattery() -> void:
	
	var batteryPercent = lights.battery_life_in_seconds / lights.battery_life_in_seconds_max
	if !_can_display:
		batteryIcon.texture = null
		return
	
	if(batteryPercent > 0.8):
		batteryIcon.texture = textures[4]
	elif(batteryPercent > 0.6):
		batteryIcon.texture = textures[3]
	elif(batteryPercent > 0.4):
		batteryIcon.texture = textures[2]
	elif(batteryPercent > 0.1):
		batteryIcon.texture = textures[1]
	elif(batteryPercent > 0):
		battery_icon_flash = !battery_icon_flash
		if(battery_icon_flash):
			batteryIcon.texture = textures[1]
		else:
			batteryIcon.texture = textures[0]
	else:
		batteryIcon.texture = textures[0]
