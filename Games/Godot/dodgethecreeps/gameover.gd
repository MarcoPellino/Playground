extends Control

signal gameover_isHiding

@onready var leaderboard = $Leaderboard
@onready var player_name_input = $PlayerNameInput
@onready var player_score_label = $PlayerScoreLabel

var current_score: int = 0
var player_name: String = "Player"
var leaderboard_data: Array = []

func _ready():
	leaderboard_data = load_leaderboard()

func setup(score: int):
	$Leaderboard/LeaderboardContainer/Leaderboard.hide()
	current_score = score
	
	# Carica leaderboard esistente
	const SAVE_PATH := "user://leaderboard.save"
	const MAX_ENTRIES := 5

	# Se meno di 5 punteggi o score è più alto dell'ultimo → si qualifica
	var qualifies = leaderboard_data.size() < MAX_ENTRIES or (leaderboard_data.size() > 0 and score > leaderboard_data[-1]["score"])

	if qualifies:
		player_score_label.text = "%d" % current_score
		player_name_input.text = ""
		player_name_input.show()
		player_score_label.show()
		player_name_input.grab_focus()
	else:
		player_name_input.hide()
		player_score_label.hide()

func load_leaderboard() -> Array:
	const SAVE_PATH := "user://leaderboard.save"

	if FileAccess.file_exists(SAVE_PATH):
		var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
		var data = file.get_var()
		file.close()
		return data
	else:
		return []

func save_score_if_top(score: int):
	const SAVE_PATH := "user://leaderboard.save"
	const MAX_ENTRIES := 5

	var entry = {"name": player_name, "score": score}
	
	leaderboard_data.append(entry)
	leaderboard_data.sort_custom(func(a, b): return a["score"] > b["score"])
	if leaderboard_data.size() > MAX_ENTRIES:
		leaderboard_data.resize(MAX_ENTRIES)

	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	file.store_var(leaderboard_data)
	file.close()


func _on_leaderboard_is_hiding() -> void:
	gameover_isHiding.emit()


func _on_player_name_input_text_submitted(new_text: String) :
	player_name = new_text
	save_score_if_top(current_score)
	$Leaderboard.update_ui()
	$PlayerScoreLabel.hide()
	$PlayerNameInput.hide()
	player_name_input.text = ""
