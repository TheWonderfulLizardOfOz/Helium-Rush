using Godot;
using System;

public partial class SleepNeed : Need
{
	
	[Export]
	int maxSleep = 100;
	[Export]
	int ticksPerDecay = 40;
	int sleep = 100;
	int decayCountdown = 40;
	
	public override void _Ready()
	{
		sleep = maxSleep;
		decayCountdown = ticksPerDecay;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void Tick()
    {
        base.Tick();
		decayCountdown--;
		if (decayCountdown == 0){
			decayCountdown = ticksPerDecay;
			sleep--;
		}
		if (sleep <= 10){
			GD.Print("Tired");
		}
    }
}
