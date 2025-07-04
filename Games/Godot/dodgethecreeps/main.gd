extends Node

@export var mob_scene: PackedScene
var score

func _ready():
	$OverlayUI/Leaderboard.hide()
	$OverlayUI/Gameover.hide()

func game_over():
	$Music.stop()
	$DeathSound.play()
	$ScoreTimer.stop()
	$MobTimer.stop()
	await $HUD.show_game_over()
	$OverlayUI/Gameover.setup(score)
	$OverlayUI/Gameover.show()
	$HUD.hide_score()
	
func new_game():
	score = 0
	$Music.play()
	get_tree().call_group("mobs", "queue_free")
	$Player.start($StartPosition.position)
	$livesbar.reset_lives()
	$livesbar.display()
	$livesbar.update_display()
	$StartTimer.start()
	$HUD.update_score(score)
	$HUD.show_message("Get Ready")

func _on_mob_timer_timeout():
	# Create a new instance of the Mob scene.
	var mob = mob_scene.instantiate()

	# Choose a random location on Path2D.
	var mob_spawn_location = $MobPath/MobSpawnLocation
	mob_spawn_location.progress_ratio = randf()

	# Set the mob's position to the random location.
	mob.position = mob_spawn_location.position

	# Set the mob's direction perpendicular to the path direction.
	var direction = mob_spawn_location.rotation + PI / 2

	# Add some randomness to the direction.
	direction += randf_range(-PI / 4, PI / 4)
	mob.rotation = direction

	# Choose the velocity for the mob.
	var velocity = Vector2(randf_range(150.0, 250.0), 0.0)
	mob.linear_velocity = velocity.rotated(direction)

	# Spawn the mob by adding it to the Main scene.
	add_child(mob)

func _on_score_timer_timeout():
	score += 1 
	$HUD.update_score(score)

func _on_start_timer_timeout():
	$MobTimer.start()
	$ScoreTimer.start()

func _on_hud_goto_leaderboard():
	#var leaderboard_scene = preload("res://leaderboard.tscn").instantiate()
	$OverlayUI/Leaderboard.show()
	$HUD.hide()

func _on_leaderboard_is_hiding() -> void:
	$OverlayUI/Leaderboard.hide()
	$HUD.show()

func _on_gameover_gameover_is_hiding() -> void:
	$OverlayUI/Gameover.hide()
	$HUD.reset_UI()
	score = 0
