extends AttackAbility
onready var explosion: AudioStreamPlayer2D = $explosion
onready var beam_out: AudioStreamPlayer2D = $beam_out
onready var sparks: AudioStreamPlayer2D = $sparks
export var dialogue : Resource
signal screen_flash
onready var battle_song: AudioStreamPlayer = $"../Intro/BattleSong"

func _ready() -> void:
	character.listen("zero_health",self,"start")

func start() -> void:
	character.interrupt_all_moves()
	
	ExecuteOnce()

func _Setup() -> void:
	play_animation("defeat")
	animatedSprite.pause_mode = Node.PAUSE_MODE_PROCESS
	call_deferred("force_movement_regardless_of_direction", horizontal_velocity * -get_player_direction_relative())
	set_vertical_speed(-jump_velocity)
	explosion.play()
	emit_signals()
	battle_song.fade_out()

func emit_signals() -> void:
	character.emit_signal("death")
	Event.emit_signal("enemy_kill",character)
	GameManager.start_cutscene()
	GameManager.player.stop_charge()
	Tools.timer(0.5,"unfreeze",self,null,true)
	GameManager.pause("BossDefeat")

func _Update(delta) -> void:
	process_gravity(delta)
	if attack_stage == 0 and timer > 0.1:
		if character.is_on_floor():
			turn_and_face_player()
			play_animation("defeat_land")
			force_movement(0)
			next_attack_stage()
	
	elif attack_stage == 1 and has_finished_last_animation():
		play_animation("defeat_loop")
		sparks.play()
		next_attack_stage()
	
	elif attack_stage == 2 and timer > 1.5:
		start_dialog_or_go_to_attack_stage(4)

	elif attack_stage == 3:
		if seen_dialog():
			next_attack_stage()
			
	elif attack_stage == 4:
		play_animation("teleport")
		beam_out.play()
		sparks.stop()
		next_attack_stage()

	elif attack_stage == 5 and has_finished_last_animation():
		animatedSprite.position.y -= 480 * delta
		if timer > 1.55:
			GameManager.end_boss_death_cutscene()
			next_attack_stage()
			emit_signal("screen_flash")
			character.destroy()

func _Interrupt() -> void:
	push_error("Interrupted Red's Death")

func unfreeze() -> void:
	GameManager.unpause("BossDefeat")
	
func start_dialog_or_go_to_attack_stage(skip_dialog_stage := 0) -> void:
	if not seen_dialog():
		GameManager.start_dialog(dialogue)
		next_attack_stage()
	elif seen_dialog():
		go_to_attack_stage(skip_dialog_stage)

func seen_dialog() -> bool:
	return GameManager.was_dialogue_seen(dialogue)
