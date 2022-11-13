extends Node
# Find the best approximate path given several changing observations. 
# From unstructured distances to an approximate ordered path, using A*
# Distance array from sensors. 

export(NodePath) var sensor_path : NodePath = ""
onready var sensor = get_node(sensor_path)

var distances = []
var locations = []
var approx_path = []
var solver = AStar.new()

var update_time = 5

var objective_location = Vector3.ZERO

func _ready():
	# Sensor setup
	sensor._spawn_nodes()
	#update()

func get_objective_weight():
	#This function is technically a placeholder, as we don't always know 
	#the location of the objective
	#This should be inferred by the agent sooner than later, but risks per-scenario overfitting
	for ray in sensor.rays:
		if ray.is_colliding():
			if ray.get_collider() is RigidBody:
				objective_location = ray.get_collision_point()
				return
	return Vector3(rand_range(0,1),rand_range(0,1), rand_range(0,1))

func update_path():
	approx_path = solver.get_point_path(solver.get_closest_point(Vector3.ZERO), solver.get_closest_point(objective_location))

func get_next_point_in_path():
	return objective_location
#	return Vector3(round(rand_range(-1,1)), round(rand_range(-1,1)), round(rand_range(-1,1)))
#	if approx_path.size() < 2:
#		return Vector3.ZERO
#	return approx_path[1]

func update():
	update_solver()
	get_objective_weight()
	update_path()
	get_tree().create_timer(5).connect("timeout", self, "update")


func update_solver():
	observe_distances()
	solver.clear() #Same object, different points. 
	for point_id in locations.size():
		#TODO: Compare both methods for weight assignment, hopefully the method for sorting positions provides better weights. 
		#solver.add_point(point_id, locations[point_id], pow(2, distances[point_id])) #This solver implies the previous ordering gets discarded
		solver.add_point(point_id, locations[point_id], pow(2, point_id)) #The further in y and then planar, the higher the exponent. 
	for point_id in locations.size():
		for point_id2 in locations.size():
			if point_id == point_id2:
				continue
			if is_zero_approx(locations[point_id].distance_to(locations[point_id2])):
				continue
			if locations[point_id].distance_to(locations[point_id2]) <= get_average_distance():
				solver.connect_points(point_id, point_id2)


func get_average_distance():
	var min_ = distances[0]
	var max_ = distances[0]
	for point in distances:
		if min_ > point:
			min_ = point
		if max_ < point:
			max_ = point
	return (min_+max_)/2.0


func observe_distances():
	locations = sensor.get_collision_points() #vector values
	distances = []
	for location in locations:
		distances.append(location.length())
	locations.sort_custom(self, "y_first_sort")

#Sorting method for 3d vectors, closer on the y coord first, then closer in the xz plane
func y_fist_sort(a, b): 
	var closer_in_y = false
	var closer_in_flat = false
	if a.y<b.y: 
		closer_in_y = true
	elif is_zero_approx(a.y - b.y):
		if Vector2(a.x, a.z).length() < Vector2(b.x, b.z).length():
			closer_in_flat = true
	return closer_in_y or closer_in_flat
			
