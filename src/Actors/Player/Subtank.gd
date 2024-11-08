extends Node

export var debug_logs := false
export (Resource) var subtank
export var active := false #
export var capacity := 16
var current_health := 0
var timer := 0.0
var last_time_increased := 0.0
var healing := false
onready var player: KinematicBody2D = $"../.."
onready var sound: AudioStreamPlayer = $heal
onready var charge: AudioStreamPlayer = $charge
onready var full: AudioStreamPlayer = $full

signal activated
signal max_capacity
signal used_at_max_capacity
signal finished_healing

func activate() -> void:
	Log("Activating, ID: " + subtank.id)
	active = true
	current_health = get_saved_health()
	Log("Current health: " + str(current_health))
	emit_signal("activated")
	Event.emit_signal("added_subtank")

func get_id() -> String:
	return subtank.id

func add_health(amount, has_played_sound) -> int:
	var exceding_amount = amount
	if current_health < capacity:
		current_health += amount
		if current_health >= capacity:
			exceding_amount = current_health - capacity
			current_health = capacity
			full.play()
			emit_signal("max_capacity")
		else:
			if not has_played_sound:
				charge.play()
			exceding_amount = 0
		Log("Added " + str(amount) + ", current: " + str(current_health))
	Log("Returning " + str(exceding_amount))
	save_health()
	return exceding_amount

func use() -> void:
	if current_health > 0 and player.current_health < player.max_health:
		Log("Starting heal, current: " + str(current_health))
		healing = true
		if current_health >= capacity:
			emit_signal("used_at_max_capacity")

func _physics_process(delta: float) -> void:
	if healing:
		timer += delta
		if player.current_health < player.max_health and current_health > 0:
			do_heal()
		else:
			current_health = 0
			last_time_increased = 0
			timer = 0
			save_health()
			healing = false
			emit_signal("finished_healing")
			subtank.emit_signal("finished_healing")
			Log("Finished, cleared tank.")

func do_heal():
	if timer > last_time_increased + 0.06:
		player.recover_health(1)
		last_time_increased = timer
		current_health -= 1
		sound.play()
		save_health()
		Event.emit_signal("subtank_health_restore")
		Log("Healed, remaining: " + str(current_health))

func get_saved_health() -> int:
	if GlobalVariables.get(subtank.id) != null:
		return GlobalVariables.get(subtank.id)
	return 0
	
func save_health():
	GlobalVariables.set(subtank.id,current_health)

func Log(message) -> void:
	if debug_logs == true:
		print_debug(name +": "+ str(message))
