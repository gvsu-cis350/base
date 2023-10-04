extends CharacterBody2D

@export var speed: int = 35
@onready var animations = $AnimationPlayer
@onready var walkUpSprite = $walkUp
@onready var walkDownSprite = $walkDown
@onready var walkRightSprite = $walkRight
@onready var walkLeftSprite = $walkLeft
@onready var idleSprite = $Idle

var direction = "Down"

func handleInput():
	var moveDirection = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	velocity = moveDirection * speed	

func hideSprites():
	walkUpSprite.hide()
	walkDownSprite.hide()
	walkRightSprite.hide()
	walkLeftSprite.hide()
	idleSprite.hide()

func updateAnimation(direction):
	hideSprites()
	# var direction = "Down"
	if velocity.length() == 0:
		idleSprite.show()
		animations.play("idle" + direction)
	else:
		if velocity.x > 0: 
			walkRightSprite.show()
			direction = "Right"
		elif velocity.x < 0: 
			walkLeftSprite.show()
			direction = "Left"
		elif velocity.y < 0: 
			walkUpSprite.show()
			direction = "Up"
		else:
			walkDownSprite.show()
			direction = "Down"
		animations.play("walk" + direction)
	
	return direction

func _physics_process(delta):
	handleInput()
	move_and_slide()
	direction = updateAnimation(direction)
