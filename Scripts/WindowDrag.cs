using Godot;
using System;

public partial class WindowDrag : Control
{
	bool isFollowing = false;
	Vector2I draggingStartPos;
	// Called when the node enters the scene tree for the first time.
public void _on_gui_input(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseButtonEvent = @event as InputEventMouseButton;
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
            {
                isFollowing = !isFollowing;
                draggingStartPos = (Vector2I)GetGlobalMousePosition();
            }
        }
    }

    public override void _Process(double delta)
    {
        if (isFollowing)
        {
            DisplayServer.WindowSetPosition((Vector2I)DisplayServer.WindowGetPosition() + (Vector2I)GetGlobalMousePosition() - draggingStartPos);
        }
    }
}
