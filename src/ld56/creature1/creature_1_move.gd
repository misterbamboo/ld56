class_name CreatureMove extends Node3D

@export var base_detection_radius: float = 3.0

@export var path_points: Array[Node3D] = [];
var path_points_distance: Array[float] = [];

var path_index = 0
var path_count = 0
var t = 0.0

var player: Player

var player_in_range: bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	player = get_node("../Player")
	
	path_count = len(path_points)
	for i in range(0, path_count):
		var nextIndex = _next_index(i)
		var diff = path_points[i].position - path_points[nextIndex].position
		path_points_distance.append(diff.length())
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	_check_player_distance()
	if player_in_range:
		_move_to_player(delta)
		_look_at_player()
	else:
		_move_on_path(delta)
	
func _move_to_player(delta: float):
	var playerAtLevel = Vector3(player.position.x, position.y, player.position.z)
	var direction = playerAtLevel - position
	var movement = direction.normalized() * delta
	position += movement
	
func _look_at_player():
	var direction = player.position - position
	var look_pos = position - direction
	look_at(look_pos)
	
func _move_on_path(delta: float):
	if(len(path_points_distance) == 0):
		return
	
	t += delta
	var currentDistanceToDo = path_points_distance[path_index]
	
	var nextIndex = _next_index(path_index)
	var fromPoint = path_points[path_index].position
	var toPoint = path_points[nextIndex].position
	
	var weight = clampf(t / currentDistanceToDo, 0, 1)
	var currentPoint = fromPoint.slerp(toPoint, weight)
	position = currentPoint
	look_at(fromPoint)
	
	if(weight >= 1):
		t -= currentDistanceToDo
		path_index = _next_index(path_index)
	
func _check_player_distance():
	var playerAtLevel = Vector3(player.position.x, position.y, player.position.z)
	var distance = (playerAtLevel - position).length()
	
	player_in_range = distance < base_detection_radius
	
	var distance_text = ""
	if player_in_range:
		distance_text = "InRange"
	
	StatsScreen.distance(var_to_str(distance) + distance_text)
	
func _next_index(index: int) -> int:
	var nextIndex = index + 1
	if nextIndex >= path_count:
		nextIndex = 0
	return nextIndex
