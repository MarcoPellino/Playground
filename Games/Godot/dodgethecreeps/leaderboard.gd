extends Control

const SAVE_PATH := "user://leaderboard.save"
const MAX_ENTRIES := 5

# Valori di default se il file non esiste
var default_entries := [
	{"name": "Anna", "score": 0},
	{"name": "Luca", "score": 0},
	{"name": "Sara", "score": 0},
	{"name": "Giulio", "score": 0},
	{"name": "Elena", "score": 0}
]

func _ready():
	var leaderboard = load_leaderboard()
	for i in MAX_ENTRIES:
		var label_name = "Score%d" % (i + 1)
		var label = $LeaderboardContainer.get_node(label_name)
		var entry = leaderboard[i]
		label.text = "%d. %s - %d" % [i + 1, entry["name"], entry["score"]]

func load_leaderboard() -> Array:
	if FileAccess.file_exists(SAVE_PATH):
		var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
		var data = file.get_var()
		file.close()

		# Se meno di 5 entry, completa con default
		while data.size() < MAX_ENTRIES:
			data.append(default_entries[data.size()])
		return data
	else:
		return default_entries.duplicate()


func _on_back_pressed():
	queue_free()
