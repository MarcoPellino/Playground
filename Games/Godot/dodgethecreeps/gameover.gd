extends Control

@onready var score_label = $ScoreLabel
@onready var leaderboard = $Leaderboard
var current_score: int = 0
var player_name: String = "Player"

func setup(score: int):
	current_score = score
	score_label.text = "Punteggio: %d" % current_score
	save_score_if_top(current_score)
	$leaderboard.update_ui()

func save_score_if_top(score: int):
	const SAVE_PATH := "user://leaderboard.save"
	const MAX_ENTRIES := 5

	var entry = {"name": player_name, "score": score}
	var data: Array = []
	if FileAccess.file_exists(SAVE_PATH):
		var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
		data = file.get_var()
		file.close()

	data.append(entry)
	data.sort_custom(func(a, b): return a["score"] > b["score"])
	if data.size() > MAX_ENTRIES:
		data.resize(MAX_ENTRIES)

	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	file.store_var(data)
	file.close()
