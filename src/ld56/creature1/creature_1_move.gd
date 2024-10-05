extends Node3D

@export var path_points: Array[Node3D] = [];
var path_points_distance: Array[float] = [];

var path_index = 0
var path_count = 0
var t = 0.0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	path_count = len(path_points)
	
	for i in range(0, path_count):
		var nextIndex = _next_index(i)
		var diff = path_points[i].position - path_points[nextIndex].position
		path_points_distance.append(diff.length())
	
	print(len(path_points_distance))
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
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
	
	if(weight >= 1):
		t -= currentDistanceToDo
		path_index = _next_index(path_index)
	
func _next_index(index: int) -> int:
	var nextIndex = index + 1
	if nextIndex >= path_count:
		nextIndex = 0
	return nextIndex
