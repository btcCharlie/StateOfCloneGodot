using Godot;
using System;
using System.Threading.Tasks;

public partial class Lightning : OmniLight3D
{
    private float lastTick;

    [Export] private float threshold = 0.9f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lastTick = Time.GetTicksMsec();
        Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // float timeDiff = Time.GetTicksMsec() - lastTick;

        if (GD.Randf() > threshold)
        {
            EnableAndDisableNode(0.2f);
        }

    }

    private async void EnableAndDisableNode(float durationInSeconds)
    {
        Visible = true;
        await Task.Delay(TimeSpan.FromSeconds(durationInSeconds));
        Visible = false;
    }
}
