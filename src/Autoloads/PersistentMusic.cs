using Godot;
using System;

public class PersistentMusic : Node
{
    private readonly AudioStreamPlayer musicPlayer = new AudioStreamPlayer();
    public override void _Ready()
    {
        musicPlayer.Bus = "Music";
        AddChild(musicPlayer);
    }

    public void SetStream(string audioPath, bool playAutomatically = true)
    {
        AudioStream audioStream = ResourceLoader.Load<AudioStream>(audioPath);
        if (audioStream != null)
        {
            musicPlayer.Stream = audioStream;
            if (playAutomatically) musicPlayer.Play();
        }
        else
        {
            GD.PrintErr("Failed to load audio: " + audioPath);
        }
    }

    public void PlayMusic()
    {
        musicPlayer.VolumeDb = 0;
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }
    public async void FadeOutMusic(float seconds) 
    {
        musicPlayer.CreateTween().TweenProperty(musicPlayer,"volume_db",-80,seconds);
        await ToSignal(GetTree().CreateTimer(seconds),"timeout");
        musicPlayer.Stop();
    }

    public void FadeInMusic(float seconds) 
    {
        musicPlayer.VolumeDb = -80;
        musicPlayer.CreateTween().TweenProperty(musicPlayer,"volume_db",0,seconds);
        musicPlayer.Play();
    }

    public bool IsPlaying()
    {
        return musicPlayer.Playing;
    }
}
