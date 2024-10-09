extends Control

#@onready var progressBar = $ProgressBar
#var player: Player
#var opacity = 0.0

# Called when the node enters the scene tree for the first time.
#func _ready() -> void:
	#player = find_parent("Player")
	#modulate = Color(1,1,1,0)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
	#if(player.is_out_of_breath):
		#opacity = sin(player.stamina*5)/2 + 0.5
		#modulate = Color(1,1,1,opacity)
	#elif player.stamina < player.total_stamina_in_seconds:
		#if opacity < 1:
			#opacity += delta
			#modulate = Color(1,1,1,opacity)
			#if opacity > 1: opacity = 1
		#
	#elif player.stamina >= player.total_stamina_in_seconds:
		#if opacity > 0:
			#opacity -= delta
			#modulate = Color(1,1,1,opacity)
			#if opacity <= 0: opacity = 0
	#
	#progressBar.value = player.stamina / player.total_stamina_in_seconds
