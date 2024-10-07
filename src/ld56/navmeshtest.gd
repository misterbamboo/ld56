extends Node3D

func gameStart() -> void:
	GameManager.raise(Events.GameStart);
