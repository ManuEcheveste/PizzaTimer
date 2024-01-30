extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	get_viewport().transparent_bg = true
	
func setBorderless():
	ProjectSettings.set_setting("display/window/size/borderless", true)
	ProjectSettings.save()
