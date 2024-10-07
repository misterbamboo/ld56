extends Node
	
var _registered_callbacks = {}

var _player_alive: bool = true
var _player: Player

var money: int = 0
func get_money() -> int:
	return money
	
func give_money(amount: int) -> void:
	money += amount
	GameManager.raise("moneyreceived")

func register_player(player: Player) -> void:
	_player = player
	
func get_player() -> Player:
	return _player
	
func _process(_delta: float) -> void:
	pass

func register(eventName: String, callback: Callable) -> void:
	if !_registered_callbacks.has(eventName):
		_registered_callbacks[eventName] = Array()
		
	var callbacks: Array = _registered_callbacks[eventName]
	if !callbacks.has(callback):
		callbacks.append(callback)

func raise(eventName: String) -> void:
	print("raise: " + eventName)
	if !_registered_callbacks.has(eventName):
		return
	
	print_rich("[color=cyan]raised event "  + eventName + "![/color]")
	var callbacks: Array = _registered_callbacks[eventName]
	for callback: Callable in callbacks:
		if callback != null:
			callback.call()

func reset_game() -> void:
	get_tree().reload_current_scene()
	_registered_callbacks.clear()
	_player_alive = true
	GameManager.raise("titlescreen")
	
func is_player_hitable() -> bool:
	return _player_alive

func player_die() -> void:
	_player_alive = false
	GameManager.raise("gameover")
