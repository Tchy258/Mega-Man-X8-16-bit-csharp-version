using Godot;
using System;

public class IntroAlysson : Node2D
{
	private AnimationPlayer animPlayer;
	private bool ableToExit = false;
	private bool exiting = false;
	private AudioStreamPlayer jingle;

	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		jingle = GetNode<AudioStreamPlayer>("Jingle");
	}

	public void SetAbleToExit() 
	{
		ableToExit = true;
	}
	public override void _Input(InputEvent @event)
	{
		if (!ableToExit) return;
		if (@event.IsActionPressed("fire") || @event.IsActionPressed("pause"))
		{
			FadeOut();
		}
	}
	public void FadeOut()
	{
		if (!exiting)
		{

			exiting = true;
			float currentTime = animPlayer.CurrentAnimationPosition;
			float remaining = animPlayer.CurrentAnimationLength - currentTime;
			remaining = remaining >= 0 ? remaining : animPlayer.CurrentAnimationLength;
			jingle.CreateTween().TweenProperty(jingle, "volume_db",-50, remaining - 0.5f);
			animPlayer.Play("FadeOut");
			animPlayer.Advance(remaining);
		}
	}
	
	public void NextScreen()
	{
		GetTree().ChangeScene("res://src/Title/IntroCapcom.tscn");
	}
}
