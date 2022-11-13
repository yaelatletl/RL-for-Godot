extends Spatial


export(NodePath) var player_path : NodePath = ""
export(Array, PackedScene) var scenarios : Array = [] #Randomize the training scenarios, avoid overfitting


onready var player : KinematicBody = get_node(player_path)
var objective
var reset_areas : Array = []

var initial_objective_state = {}

func register_objective_state():
	initial_objective_state["position"] = objective.global_transform.origin
	initial_objective_state["rotation"] = objective.global_transform.basis
	initial_objective_state["linear_velocity"] = objective.linear_velocity
	initial_objective_state["angular_velocity"] = objective.angular_velocity

func reset_objective_state():
	objective.global_transform.origin = initial_objective_state["position"]
	objective.global_transform.basis = initial_objective_state["rotation"]
	objective.linear_velocity = initial_objective_state["linear_velocity"]
	objective.angular_velocity = initial_objective_state["angular_velocity"]

func _ready():
	randomize()
	var scene = scenarios[randi() % scenarios.size()].instance()
	$Structure.add_child(scene)
	var scene_data = scene.get_data()
	objective = scene_data[0]
	var reset_nodes = scene_data[1]
	for reset in reset_nodes:
		if not is_instance_valid(reset):
			continue
		reset_areas.append(reset)
		reset.connect("body_entered", self, "_on_Reset_body_entered")
	register_objective_state()
	objective.connect("body_entered", self, "_on_Goal_Trigger_body_entered")



func _on_Reset_body_entered(body):
	reset_objective_state()
	player.done = true
	player.reset()
	player.pathfinder.objective_location = objective.global_transform.origin

func _on_Goal_Trigger_body_entered(body):
	if body is KinematicBody:
		body.reward += 100.0
		body.done = true
		body.just_reached_end = true
		body.needs_reset = true
		reset_objective_state()
	