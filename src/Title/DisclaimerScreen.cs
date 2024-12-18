using Godot;
using System;

public class DisclaimerScreen : Control
{
    private Sprite fade;
    private readonly TweenController tween;
    private Label inspired;
    private bool exiting = false;

    public DisclaimerScreen() 
    {
        tween = new TweenController(this, false);
    }
    public override void _Ready()
    {
        fade = GetNode<Sprite>("Fade");
        fade.Modulate = Color.ColorN("black");
        inspired = GetNode<Label>("Inspired");
        UserDataFile data = GetNode<UserDataFile>("/root/UserDataFile");
        string storedLanguage = data.CurrentLanguage;
        data.SetLanguage(storedLanguage);
        TranslationServer.SetLocale(storedLanguage);
        inspired.Visible = true;
        GetTree().CreateTimer(0.5f).Connect("timeout",this,"FadeIn");
        GetTree().CreateTimer(10.0f).Connect("timeout",this,"FadeOut");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("pause")) 
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        if (!exiting) 
        {
            inspired.Modulate = Color.ColorN("darkblue");
            tween.Property(
                fade,
                "modulate:a",
                0.0f,
                0.5f
            );
            tween.AddProperty(
                inspired,
                "modulate",
                Color.ColorN("white"),
                0.5f
            );
        }
    }

    public void FadeOut()
    {
        if (!exiting) 
        {
            exiting = true;
            tween.Reset();
            tween.Property(inspired,"modulate",Color.ColorN("darkblue"),0.5f);
            tween.AddProperty(fade, "modulate:a", 1.0f, 0.5f);
            tween.AddWait(0.5f);
            tween.AddCallback(this,"NextScreen");
        }
    }

    public void NextScreen()
    {
        GetTree().ChangeScene("res://src/Title/IntroAlysson.tscn");
    }
}
