extends Area2D

signal hit

@export var speed = 400 # How fast the player will move (pixels/sec).
var screen_size # Size of the game window.
var isImmune = false

func _ready():
	screen_size = get_viewport_rect().size
	hide()
	
func _process(delta):
	var velocity = Vector2.ZERO # The player's movement vector.
	if Input.is_action_pressed("move_right"):
		velocity.x += 1
	if Input.is_action_pressed("move_left"):
		velocity.x -= 1
	if Input.is_action_pressed("move_down"):
		velocity.y += 1
	if Input.is_action_pressed("move_up"):
		velocity.y -= 1

	if velocity.length() > 0:
		velocity = velocity.normalized() * speed
		$AnimatedSprite2D.play()
	else:
		$AnimatedSprite2D.stop()
		
	if velocity.x != 0:
		$AnimatedSprite2D.animation = "walk"
	elif velocity.y != 0:
		$AnimatedSprite2D.animation = "up"

	$AnimatedSprite2D.flip_h = velocity.x < 0
	$AnimatedSprite2D.flip_v = velocity.y > 0
	
	position += velocity * delta
	position = position.clamp(screen_size * 0.05, screen_size * 0.95)


func _on_body_entered(body: Node2D) -> void:
	if (isImmune):
		return
	
	$Immunity.start()
	$AnimatedSprite2D.material.set("shader_parameter/immune", true)# attiva effetto
	isImmune = true
	hit.emit()

func start(pos):
	position = pos
	show()
	$CollisionShape2D.disabled = false
	
func lives_deplated():
	hide() # Player disappears after depleting all lives
	# Must be deferred as we can't change physics properties on a physics callback.
	$CollisionShape2D.set_deferred("disabled", true)


func _on_immunity_timeout():
	isImmune = false
	$AnimatedSprite2D.material.set("shader_parameter/immune", false) # attiva effetto
