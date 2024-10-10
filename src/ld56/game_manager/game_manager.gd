extends Node
	
var _registered_callbacks = {}

var _player_alive: bool = true
var _player: Player

var _first_flashlight := true

var week_duration = 3
var current_day = 1

var first_quota = 120
var quota = first_quota

var uncashed_in_money: int = 0
var money: int = 0

func get_unchashed_in_money() ->int:
	return uncashed_in_money

func get_money() -> int:
	return money
	
func give_money(amount: int) -> void:
	uncashed_in_money += amount
	GameManager.raise(Events.MoneyReceived)

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
	if !_registered_callbacks.has(eventName):
		return
	
	print_rich("[color=cyan]raised event "  + eventName + "![/color]")
	var callbacks: Array = _registered_callbacks[eventName]
	for callback: Callable in callbacks:
		if callback != null:
			callback.call()

func reset_game(newGame:bool) -> void:
	get_tree().reload_current_scene()
	_registered_callbacks.clear()
	_player_alive = true
	
	if newGame:
		quota = first_quota
		money = 0
		uncashed_in_money = 0
	else:
		GameManager.raise(Events.GameStart)

func next_day() -> void:
	current_day = current_day + 1
	if current_day == week_duration+1:
		current_day = 1 
		if quota_reached():
			quota = quota * 1.7
			cash_in_money()
			proceed_to_next_day()
		else:
			raise(Events.GameOver)
	else:
		proceed_to_next_day()

func quota_reached() -> bool:
	return uncashed_in_money > quota
	
func proceed_to_next_day():
	if _player.is_inside || !_player_alive:
		uncashed_in_money = 0
	reset_game(false)
	
func cash_in_money()->void:
	money += uncashed_in_money
	uncashed_in_money = 0

func is_player_hitable() -> bool:
	return _player_alive

func player_die() -> void:
	if(!_player_alive): return
	_player_alive = false
	GameManager.raise(Events.EndDay)
	
func set_action(label: String) -> void:
	_player._set_action(label)
