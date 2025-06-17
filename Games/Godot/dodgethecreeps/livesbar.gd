extends HBoxContainer

signal game_over

var max_lives := 3
var current_lives := 3

func _ready():
	hide()

func display():
	show()

func lose_life():
	current_lives -= 1
	update_display()

	if current_lives <= 0:
		emit_signal("game_over")

func update_display():
	for i in range(get_child_count()):
		var icon = get_child(i)
		icon.visible = i < current_lives

func reset_lives():
	current_lives = max_lives
	update_display()
