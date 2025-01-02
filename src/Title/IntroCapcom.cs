using Godot;
using System;

public class IntroCapcom : Node
{
    
    private bool ending = false;
    private bool goToMainMenu = false;
    private AnimatedSprite capcomLogo;
    private AudioStreamPlayer capcomSound;
    private Label inspiredBy;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        capcomLogo = GetNode<AnimatedSprite>("CapcomLogo");
        capcomSound = GetNode<AudioStreamPlayer>("CapcomSound");
        inspiredBy = GetNode<Label>("InspiredBy");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Skip()
    {
        capcomSound.CreateTween().TweenProperty(capcomSound,"volume_db",-80,0.5f);
        animationPlayer.Play("FadeOut");
    }

    public void SetEnding()
    {
        ending = true;
    }
    public void OnMenuButtonPressed()
    {
        if (!ending)
        {
            ending = true;
            goToMainMenu = true;
            Skip();
        }
    }

    public void NextScreen()
    {
        if (!goToMainMenu) GetTree().ChangeScene("res://src/Title/IntroAnim.tscn");
        else GetTree().ChangeScene("res://src/Options/MainMenu.tscn");
    }

}
