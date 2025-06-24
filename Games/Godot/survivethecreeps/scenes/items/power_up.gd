extends Control

var data: PowerUpData

func init(power_up_data: PowerUpData) -> void:
	data = power_up_data
	_update_ui()

func _update_ui():
	$Text.text = data.name
	$Cost.text = str(data.price)
	$Icon.texture = data.icon
	$Description.text = data.description
