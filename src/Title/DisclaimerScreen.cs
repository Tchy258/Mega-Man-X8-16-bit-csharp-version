using Godot;
using System;

public class DisclaimerScreen : Control
{
	private bool exiting = false;
	private AnimationPlayer animPlayer;

	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		UserDataFile data = GetNode<UserDataFile>("/root/UserDataFile");
		string storedLanguage = data.CurrentLanguage;
		data.SetLanguage(storedLanguage);
		TranslationServer.SetLocale(storedLanguage);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("pause")) 
		{
			FadeOut();
		}
	}
	public void FadeOut()
	{
		if (!exiting && animPlayer.CurrentAnimationPosition > 1f) 
		{
			exiting = true;
			animPlayer.Seek(11f,true);
		}
	}

	public void NextScreen()
	{
		GetTree().ChangeScene("res://src/Title/IntroAlysson.tscn");
	}
}
