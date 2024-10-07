extends Control

@onready var label = $Label
@onready var daysLeftLabel = $DaysLeftLabel
@onready var uncashedProfitsLabel = $UncashedProfitsLabel

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	GameManager.register(Events.EndDay, appear_screen)
	GameManager.register(Events.GameOver, hide_screen)
	hide_screen()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass

func appear_screen() -> void:
	if !GameManager._player_alive:
		label.text = "You died in the mines, all uncashed profits lost."
	elif GameManager._player.is_inside:
		label.text = "The truck left without you, \nall uncashed profits lost."
	else:
		label.text = "Another day, another trip to the mines"
		uncashedProfitsLabel.text = "%d $" % GameManager.uncashed_in_money
	
	daysLeftLabel.text = "%d" % (GameManager.week_duration - GameManager.current_day)
	visible = true

func _on_button_pressed() -> void:
	GameManager.next_day()

func hide_screen() -> void:
	visible = false
