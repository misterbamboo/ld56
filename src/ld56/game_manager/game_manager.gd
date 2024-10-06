extends Node
	
var _registered_callbacks = {}

#func _ready() -> void:
	#pass
	
func _process(delta: float) -> void:
	if Input.is_key_pressed(KEY_R):
		GameManager.raise("gameover")

func register(eventName: String, callback: Callable) -> void:
	if !_registered_callbacks.has(eventName):
		_registered_callbacks[eventName] = Array()
		
	var callbacks: Array = _registered_callbacks[eventName]
	if !callbacks.has(callback):
		callbacks.append(callback)

func raise(eventName: String) -> void:
	if !_registered_callbacks.has(eventName):
		return
	
	var callbacks: Array = _registered_callbacks[eventName]
	for callback: Callable in callbacks:
		if callback != null:
			callback.call()

func reset_game() -> void:
	get_tree().reload_current_scene()
	_registered_callbacks.clear()
