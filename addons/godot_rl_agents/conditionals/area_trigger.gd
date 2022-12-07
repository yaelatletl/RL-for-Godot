extends Area
class_name RewardArea
#Export variables, we need to define our reward or penalty values
export(float) var value = 1.0
export(bool) var is_penalty = false
export(bool) var is_reset = false
export(bool) var is_one_shot = false

var disabled = false

signal triggered 

func _ready() -> void:
	connect("body_entered", self, "_on_body_entered")

func _on_body_entered(body) -> void:
	if body is GenericAgent and not disabled:
		if is_penalty:
			body.apply_penalty(value)
		else:
			body.apply_reward(value)
		if is_one_shot:
			disabled = true
		if is_reset:
			body.set_done_true()
			body.reset_if_done()
		emit_signal("triggered")

func reset() -> void:
	disabled = false