extends Node2D

@export var powerup_scene: PackedScene
@onready var container = $ItemsContainer/Items # adatta questo percorso

func _ready():
	var powerups = load_powerups_from_csv("res://resources/settings/shop.txt")
	for data in powerups:
		var item = powerup_scene.instantiate()
		item.init(data)
		container.add_child(item)

func load_powerups_from_csv(path: String) -> Array[PowerUpData]:
	var file := FileAccess.open(path, FileAccess.READ)
	if not file:
		push_error("Could not open powerup file: %s" % path)
		return []
	
	var readed = file.get_as_text()
	file.close()
	var lines = readed.split("\n")
	var header = lines[0].split(",")

	var powerups: Array[PowerUpData] = []
	for i in range(1, lines.size()):
		var values = lines[i].split(",")
		var data = PowerUpData.new()
		data.name = values[0]
		data.description = values[1]
		data.price = values[2].to_int()
		data.icon = load(values[3].strip_edges())
		powerups.append(data)
	return powerups
