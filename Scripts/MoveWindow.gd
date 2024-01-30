extends Control

#var following = false
#var dragging_start_position: Vector2i = DisplayServer.mouse_get_position()

#func _on_gui_input(event):
#	if event is InputEventMouseButton:
#		if event.get_button_index() == 1:
#			following = !following
#			dragging_start_position = DisplayServer.mouse_get_position()

#func _process(_delta):
#	if following:
#		get_window().position += DisplayServer.mouse_get_position() - dragging_start_position
