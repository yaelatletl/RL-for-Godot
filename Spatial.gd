extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	for P in [0,1]:
		for Q in [0,1]:
			for R in [0,1]:
				for S in [0,1]:
					print("P: ", P, " Q: ",Q, " R: ",R, " S: ", S," = ", (bool(P)and bool(R))and(not bool(Q) or bool(S)) or bool(S))

	for P in [0,1]:
		for Q in [0,1]:
			for R in [0,1]:
				for S in [0,1]:
					print("P: ", P, " R: ",R, " S: ", S," = ", (bool(P)and bool(R)) or bool(S))
