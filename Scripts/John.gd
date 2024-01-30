extends Sprite2D
func _ready():
	position = get_node("../Timer Fill").get_rect().position


func _process(delta):
	# inc the value of the progress bar
	get_node("../Timer Fill").value += 1
	# set the sprite's x position according to value of the bar:
	# you need to scale 'value according in the following line...
	position.x = get_node("../Timer Fill").get_rect().position.x + get_node("../Timer Fill").value
