extends CanvasLayer

# Notifies `Main` node that the button has been pressed
signal start_game
signal goto_leaderboard

func show_message(text):
	$Message.text = text
	$Message.show()
	$MessageTimer.start()
	
func show_game_over():
	show_message("Game Over")
	# Wait until the MessageTimer has counted down.
	await $MessageTimer.timeout
	
func update_score(score):
	$ScoreLabel.text = str(score)

func hide_score():
	$ScoreLabel.hide()
	
func show_score():
	$ScoreLabel.show()
	
func reset_UI():
	$Message.text = "Dodge the Creeps!"
	$Message.show()
	# Make a one-shot timer and wait for it to finish.
	await get_tree().create_timer(1.0).timeout
	$StartButton.show()
	$Leaderboard.show()
	show_score()

func _on_start_button_pressed() -> void:
	$StartButton.hide()
	$Leaderboard.hide()
	start_game.emit()

func _on_leaderboard_button_pressed():
	goto_leaderboard.emit()

func _on_message_timer_timeout() -> void:
	$Message.hide()
