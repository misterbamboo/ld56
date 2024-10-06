extends Node3D

func gameStart() -> void:
	GameManager.raise("gamestart");
