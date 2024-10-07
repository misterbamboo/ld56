extends Node3D

@export var depth := 1 # 1 à 4

@export var gold_ring : PackedScene
@export var emerald: PackedScene
@export var amethyst: PackedScene
@export var kryptonite: PackedScene


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	GameManager.register(Events.StartDay, spawnItem)

func spawnItem() -> void:
	# 1 in 10 chance
	if randi() % 10 != 0: return
	
	var rand = randi()%100
	match depth:
		1:
			if rand < 75:
				spawn(emerald)
				return
			else:
				spawn(gold_ring)
		2:
			if rand < 50:
				spawn(emerald)
			elif rand < 75:
				spawn(gold_ring)
			else:
				spawn(amethyst)
		3:
			if rand < 25:
				spawn(emerald)
			elif rand < 50:
				spawn(gold_ring)
			elif rand < 75:
				spawn(amethyst)
			else:
				spawn(kryptonite)
		4:
			if(rand < 10):
				spawn(emerald)
			if rand < 30:
				spawn(gold_ring)
			if(rand < 65):
				spawn(amethyst)
			else:
				spawn(kryptonite)

func spawn(scene:PackedScene) -> void:
	print("im spawning a " + scene.resource_name)
	var item = scene.instantiate() as Node3D;
	item.position = position
	get_parent().add_child(item);

# add icon to editor 3d view with plugin
static func _dev_icon() -> Dictionary:
	return {
		&"path": "res://items/item_spawner_icon.png",
		&"size": 0.5
	}
