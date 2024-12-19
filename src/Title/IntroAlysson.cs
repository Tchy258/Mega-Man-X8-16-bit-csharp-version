using Godot;
using System;

public class IntroAlysson : Node2D
{
    private AnimationPlayer animPlayer;
    private bool ableToExit = false;
    private bool exiting = false;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
        if (!exiting && animPlayer.CurrentAnimationPosition < 6f)
        {
            exiting = true;
            animPlayer.Play("FadeOut");
        }
    }
    
    public void NextScreen()
    {
        GetTree().ChangeScene("res://src/Title/IntroCapcom.tscn");
    }
}
