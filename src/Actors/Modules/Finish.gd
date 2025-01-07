extends EventAbility
class_name Finish

export var beam_speed := 420.0
var stage_clear_song_duration := 3.8
var ascending := false
var victoring := false
onready var animatedSprite = get_parent().get_node("animatedSprite")
onready var victory_sound = get_node("audioStreamPlayer")
onready var beam_out_sound = get_node("audioStreamPlayer2")

var victory_pose_and_music := true

func _ready() -> void:
	animatedSprite.connect("animation_finished",self,"on_animation_finished")
	Event.connect("disable_victory_ending",self,"disable_victory")

func disable_victory():
	victory_pose_and_music = false

func _Setup():
	character.deactivate()
	character.stop_all_movement()
	if victory_pose_and_music:
		Event.emit_signal("play_stage_clear_music")

func _Update(_delta):
	process_gravity(_delta)
	if not victoring:
		if character.is_on_floor():
			play_animation_once("idle")
			if not victory_pose_and_music:
				if timer > 1.5:
					character.play_animation_backwards("beam_in")
					victoring = true
					if IGT.can_split:
						IGT.calculate_total_time()
						IGT.send_command("split_command")
						IGT.can_split = false
		
		if victory_pose_and_music:
			if timer > stage_clear_song_duration:
				play_animation_once("victory")
				victory_sound.play()
				victoring = true
				if IGT.can_split:
						IGT.calculate_total_time()
						IGT.send_command("split_command")
						IGT.can_split = false

	if ascending:
		animatedSprite.global_position.y -= beam_speed * _delta
		if timer > 1:
			GameManager.end_level()

	
func on_animation_finished():
	if executing:
		if character.get_animation() == "victory":
			character.play_animation_backwards("beam_in")

		elif character.get_animation() == "beam_in":
			play_animation_once("beam")
			beam_out_sound.play()
			timer = 0
			ascending = true

func _EndCondition() -> bool:
	return false

func is_high_priority() -> bool:
	return true

		
