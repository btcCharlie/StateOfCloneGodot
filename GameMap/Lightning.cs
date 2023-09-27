using Godot;
using System;

public partial class Lightning : OmniLight3D
{
    private float lastTick;

    [Export] private float threshold = 0.9f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lastTick = Time.GetTicksMsec();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // float timeDiff = Time.GetTicksMsec() - lastTick;



    }
}
