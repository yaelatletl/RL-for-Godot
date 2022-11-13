extends Node
class_name GenericAgent

export(Array, NodePath) var sensor_paths
export(int) var MAX_STEPS = 20000
var needs_reset = false
var n_steps = 0
var reward = 0.0
var sensors = []
var done = false
var _heuristic = "player"

signal agent_has_reset()

func _reset() -> void:
	needs_reset = false
	n_steps = 0
	emit_signal("agent_has_reset")

func reset_if_done() -> void:
	if done:
		_reset()

func get_done() -> bool:
	return done
	
func set_done_false() -> void:
	done = false

func set_heuristic(heuristic) -> void:
	self._heuristic = heuristic

func update_reward() -> void:
	reward -= 0.01 # step penalty
	reward += shaping_reward()

func apply_penalty(penalty) -> void:
	reward -= abs(penalty)

func apply_reward(reward) -> void:
	reward += abs(reward)

func shaping_reward() -> float: #Virtual Method, must be replaced 
	return 0.0

func zero_reward() -> void:
	reward = 0

func _ready() -> void:
	for sensor in sensor_paths:
		if is_instance_valid(get_node(sensor)):
			sensors.append(get_node(sensor))
			get_node(sensor).activate()
	_reset()

func _physics_process(delta) -> void:
	n_steps +=1
	if n_steps >= MAX_STEPS:
		done = true
		needs_reset = true

	if needs_reset:
		_reset()
		return

	update_reward()

func get_obs() -> Dictionary: #Virtual Method, must return
	var obs = []
	return {
		"obs": obs,
	   }
	
func get_obs_space() -> Dictionary:
	# typs of obs space: box, discrete, repeated
	return {
		"obs": {
			"size": [len(get_obs()["obs"])],
			"space": "box"
			}
		}

func get_action_space() -> Dictionary:
	return {
		"action_name" : {
			 "size": 1, #1 or 2, why
			"action_type": "continuous" #continuous, discrete
			},      
		}
