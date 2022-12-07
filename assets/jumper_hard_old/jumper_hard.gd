extends Spatial

onready var agent = $Player
onready var first_jump_pad = $FirstPad
onready var second_jump_pad = $SecondPad
var next = 1
var best_goal_distance := 10000.0

func _ready():
	$FirstPad/Trigger.connect("triggered", self, "_on_First_Pad_Trigger_triggered")
	$SecondPad/Trigger.connect("triggered", self, "_on_Second_Trigger_triggered")
	$Player.connect("agent_has_reset", self, "reset")

func reset():
	reset_best_goal_distance()

func reset_best_goal_distance():
	if next == 0:
		best_goal_distance = translation.distance_to(first_jump_pad.translation)
	if next == 1:
		best_goal_distance = translation.distance_to(second_jump_pad.translation)   
	agent.best_goal_distance = best_goal_distance
	 

func calculate_translation(other_pad_translation : Vector3) -> Vector3:
	var new_translation := Vector3.ZERO
	var distance = rand_range(12,16)
	var angle = rand_range(-180,180)
	new_translation.z = other_pad_translation.z + sin(deg2rad(angle))*distance 
	new_translation.x = other_pad_translation.x + cos(deg2rad(angle))*distance
	return new_translation

func _on_First_Pad_Trigger_triggered():
	if next != 0:
		return
	#reward += 100.0 Reward is now handled by the trigger
	next = 1
	reset_best_goal_distance()
	second_jump_pad.translation = calculate_translation(first_jump_pad.translation)

func _on_Second_Trigger_triggered():
	if next != 1:
		return
	#reward += 100.0 Reward is now handled by the trigger
	next = 0
	reset_best_goal_distance()
	first_jump_pad.translation = calculate_translation(second_jump_pad.translation)
		
