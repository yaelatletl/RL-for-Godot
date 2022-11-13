extends Spatial

export(NodePath) var objective_path
export(Array, NodePath) var reset_paths

func get_data():
	var data = []
	var resets = []
	for path in reset_paths:
		resets.append(get_node(path))
	data.append(get_node(objective_path))
	data.append(resets)
	return data