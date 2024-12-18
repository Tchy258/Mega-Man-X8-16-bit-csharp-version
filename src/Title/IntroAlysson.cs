using Godot;
using System;

public class IntroAlysson : Node2D
{
    private TweenController tween;
    private Label channelName;
    private Label madeBy;
    private Sprite fade;
    private AudioStreamPlayer jingle;
    private Sprite signatureSprite;
    private bool ableToExit = false;
    private bool exiting = false;

    [Export]
    public Color color1;
    [Export]
    public Color color2;

    public override void _Ready()
    {
        fade = GetNode<Sprite>("Fade");
        signatureSprite = GetNode<Sprite>("Signature");
        tween = new TweenController(this,false);
        channelName = GetNode<Label>("ChannelName");
        madeBy = GetNode<Label>("MadeBy");
        jingle = GetNode<AudioStreamPlayer>("Jingle");
        jingle.Play();
        madeBy.Modulate = Color.ColorN("black");
        signatureSprite.Modulate = Color.ColorN("black");
        channelName.Modulate = Color.ColorN("black");
        Activate();
    }

    public void Activate() 
    {
        tween.Property(madeBy,"modulate",Color.ColorN("white"),2.0f);
        GetTree().CreateTimer(1.0f).Connect("timeout",this,"Appear");
        GetTree().CreateTimer(2.5f).Connect("timeout",this,"SetAbleToExit");
        GetTree().CreateTimer(6f).Connect("timeout",this,"Fade");
        GetTree().CreateTimer(6f).Connect("timeout",this,"FadeMadeBy");
    }

    public void Fade()
    {
        tween.Property(signatureSprite,"modulate",color2,.7f);
        tween.AddProperty(signatureSprite,"modulate",color1,.7f);
        tween.AddCallback(this,"FadeOut");
        tween.Property(channelName,"modulate",Color.ColorN("black"),2.0f);
    }
    public void FadeMadeBy()
    {
        tween.Property(madeBy,"modulate",Color.ColorN("black"),2.0f);
    }
    public void SetAbleToExit() 
    {
        ableToExit = true;
    }
    public void Appear() 
    {
        tween.Property(signatureSprite,"modulate", color1, 1.3f);
        tween.AddProperty(signatureSprite,"modulate", color2, 0.6f);
        tween.AddProperty(signatureSprite,"modulate",Color.ColorN("white"),0.9f);
        tween.Property(channelName,"modulate",Color.ColorN("black"),0.5f);
        tween.AddProperty(channelName, "modulate",Color.ColorN("white"),3f);
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
            tween.Reset();
            tween.Property(jingle,"volume_db",-50f,1f);
            tween.Property(fade,"modulate",Color.ColorN("black"),1f);
            tween.AddWait(1f);
            tween.AddCallback(this,"NextScreen");
        }
    }
    
    public void NextScreen()
    {
        GetTree().ChangeScene("res://src/Title/IntroCapcom.tscn");
    }
}
