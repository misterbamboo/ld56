using Godot;
using System;

public partial class FlickingSpot : SpotLight3D
{
	private double t;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		t += delta;

		// https://www.desmos.com/calculator/jomngsay3s
		var intensity = (Math.Sin(t) * Math.Cos(0.3 * t) * Math.Sin(50 * t) + 1 * Math.Sin(0.2 * t)) + 1;
		intensity = Math.Clamp(intensity, 0d, 1d);
		LightEnergy = (float)intensity;
	}
}
