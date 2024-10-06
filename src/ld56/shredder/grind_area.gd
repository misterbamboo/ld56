extends Area3D

@export var audioShredItem: AudioShredItem

@export var grind_start: Node3D
@export var grind_end: Node3D
@export var shake_force: float = 0.2
@export var grind_speed: float = 0.25

var grinding_items: Array[BaseItem] = []
var grinding_times: Array[float] = []

func _process(delta: float) -> void:
	if len(grinding_items) > 0:
		_grind_items(delta)

func _on_body_entered(body: Node3D) -> void:
	if body and body.get_parent() is BaseItem:
		var base_item: BaseItem = body.get_parent()
		base_item.grind()
		audioShredItem.grind_play()
		grinding_items.append(base_item)
		grinding_times.append(0)

func _grind_items(delta: float) -> void:
	var any_removed: bool = false
	for i in range(0, len(grinding_items)):
		any_removed = any_removed or _grind_item(i, delta, any_removed)
		
func _grind_item(index: int, delta: float, any_removed: bool) -> bool:
	grinding_times[index] += delta * grind_speed
	var grinding_item: BaseItem = grinding_items[index]
	var t: float = grinding_times[index]
	
	grinding_item.rb.global_position = grind_start.global_position.lerp(grind_end.global_position, t)
	
	var xrot = _shake(t);
	var zrot = _shake(t + 0.25);
	var targetRot = Vector3.ZERO + Vector3(xrot, 0, zrot)
	grinding_item.rb.rotation = targetRot
	
	# allow 1 removal by frame
	if !any_removed and t >= 1:
		grinding_items.remove_at(index)
		grinding_times.remove_at(index)
		grinding_item.destroy()
		return true
	
	return false

func _shake(t: float) -> float:
	return sin(100 * t) * cos(150 * t) * shake_force
