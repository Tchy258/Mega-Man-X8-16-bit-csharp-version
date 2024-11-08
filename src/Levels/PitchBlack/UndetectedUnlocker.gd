extends Node

export var achievement : Resource

var detected := false
var lit := false

func _ready() -> void:
	Event.connect("alarm",self,"on_alarm")
	Event.connect("pitch_black_energized",self,"on_lit")

func on_lit():
	print("Achievements: disabled " +achievement.get_id())
	lit = true

func on_alarm():
	print("Achievements: disabled " +achievement.get_id())
	detected = true

func fire_achievement() -> void:
	print("Achievements: firing achievement....")
	if not detected and not lit:
		Achievements.unlock(achievement.get_id())
