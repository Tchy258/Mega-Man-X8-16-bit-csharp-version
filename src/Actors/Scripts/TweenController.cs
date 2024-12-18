using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Utility class for controlling tweeners
/// </summary>
public class TweenController : Reference
{
    /// <summary>
    /// List of SceneTreeTweens currently running on ownerNode
    /// </summary>
    private List<SceneTreeTween> tweenList;
    /// <summary>
    /// The node that requested tweening
    /// </summary>
    private readonly Node ownerNode;

    /// <summary>
    /// Creates an instance of a TweenController
    /// </summary>
    /// <param name="owner">Node that needs tweening.</param>
    /// <param name="connect">Whether to connect a "Stop" signal to the Reset method.</param>
    public TweenController(Node owner, bool connect = true)
    {
        ownerNode = owner;
        tweenList = new List<SceneTreeTween>();
        if (connect) ConnectReset();
    }

    /// <summary>
    /// Connects end_signal from the owner node to the Reset method
    /// </summary>
    /// <param name="end_signal">Signal of the owner node to listen to</param>
    public void ConnectReset(string end_signal = "Stop") 
    {
        ownerNode.Connect(end_signal, this, "Reset");
    }

    /// <summary>
    /// Creates a SceneTreeTween with the given properties and adds it to tweenList.
    /// </summary>
    /// <param name="easeType"> Tween ease type, InOut by default</param>
    /// <param name="transitionType">Tween trans type, Linear by default</param>
    /// <param name="parallel">If parallel is true, the Tweeners appended after this method will by default run simultaneously, as opposed to sequentially.</param>
    /// <param name="loops">Sets the number of times the tweening sequence will be repeated</param>
    public void CreateSceneTreeTween(
        Tween.EaseType easeType = Tween.EaseType.InOut, 
        Tween.TransitionType transitionType = Tween.TransitionType.Linear,
        bool parallel = false,
        int loops = 1)
        {
            SceneTreeTween tween = ownerNode.CreateTween();
            tween.SetEase(easeType)
                 .SetTrans(transitionType)
                 .SetParallel(parallel)
                 .SetLoops(loops);
            tweenList.Add(tween);
        }

    /// <summary>
    /// Fetches the most recently created SceneTreeTween in the tweenList
    /// </summary>
    /// <returns>Last element of tweenList</returns>
    public SceneTreeTween GetLast()
    {
        return tweenList.Last();
    }
    /// <summary>
    /// Sets the number of times the tweening sequence will be repeated for the most recently created tweener
    /// </summary>
    /// <param name="value">Times to execute the animation</param>
    public void SetLoops(int value = 1)
    {
        GetLast().SetLoops(value);
    }

    /// <summary>
    /// Sets the most recently created tween's ease type and trans type
    /// </summary>
    /// <param name="easeType"></param>
    /// <param name="transitionType"></param>
    public void SetEase(Tween.EaseType easeType, Tween.TransitionType transitionType)
    {
        GetLast().SetEase(easeType).SetTrans(transitionType);
    }
    /// <summary>
    /// Wrapper for SceneTreeTween.TweenProperty on a newly created tween that is added to the list
    /// </summary>
    /// <seealso cref="SceneTreeTween.TweenProperty"/>
    /// <param name="obj"></param>
    /// <param name="property"></param>
    /// <param name="finalValue"></param>
    /// <param name="duration"></param>
    public void Property(Godot.Object obj, string property, object finalValue, float duration = 0.25f)
    {
        SceneTreeTween tween = ownerNode.CreateTween();
        tween.TweenProperty(obj, property, finalValue, duration);
        tweenList.Add(tween);
    }

    /// <summary>
    /// Wrapper for SceneTreeTween.TweenMethod on a newly created tween that is added to the list
    /// </summary>
    /// <seealso cref="SceneTreeTween.TweenMethod"/>
    /// <param name="obj"></param>
    /// <param name="method"></param>
    /// <param name="initialValue"></param>
    /// <param name="finalValue"></param>
    /// <param name="duration"></param>
    public void Method(Godot.Object obj, string method, object initialValue, object finalValue, float duration = 0.25f)
    {
        SceneTreeTween tween = ownerNode.CreateTween();
        tween.TweenMethod(obj, method, initialValue, finalValue, duration);
        tweenList.Add(tween);
    }

    /// <summary>
    /// Wrapper for SceneTreeTween.TweenCallback on a newly created tween that is added to the list
    /// </summary>
    /// <seealso cref="SceneTreeTween.TweenCallback"/>
    /// <param name="obj"></param>
    /// <param name="method"></param>
    /// <param name="binds"></param>
    /// <param name="delay"></param>
    public void Callback(Godot.Object obj, string method, Godot.Collections.Array binds, float delay = 1.0f) 
    {
        SceneTreeTween tween = ownerNode.CreateTween();
        tween.TweenCallback(obj, method, binds).SetDelay(delay);
    }

    public void AddCallback(Godot.Object obj, string method, Godot.Collections.Array binds = null) 
    {
        GetLast().TweenCallback(obj,method,binds);
    }
    
    public void AddProperty(Godot.Object obj, string property, object finalValue, float duration = 0.25f)
    {
        GetLast().TweenProperty(obj, property, finalValue, duration);
    }

    public void AddMethod(Godot.Object obj, string method, object initialValue, object finalValue, float duration, Godot.Collections.Array binds = null)
    {
        GetLast().TweenMethod(obj, method, initialValue, finalValue, duration, binds);
    }

    public void AddWait(float duration = 0.25f)
    {
        GetLast().TweenInterval(duration);
    }

    public void SetSequential()
    {
        GetLast().SetParallel(false);
    }

    public void SetParallel() 
    {
        GetLast().SetParallel(true);
    }

    public void EndAbility()
    {
        SetSequential();
        AddCallback(ownerNode, "EndAbility", null);
    }

    public void SetEaseOut()
    {
        GetLast().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
    }

    public void SetIgnorePauseMode()
    {
        GetLast().SetPauseMode(SceneTreeTween.TweenPauseMode.Process);
    }

    public void Pause() 
    {
        foreach (SceneTreeTween tween in tweenList)
        {
            if (tween.IsValid()) tween.Pause();
        }
    }
    
    public void UnPause()
    {
        foreach (SceneTreeTween tween in tweenList)
        {
            if (tween.IsValid()) tween.Play();
        }
    }

    public void End()
    {
        foreach (SceneTreeTween tween in tweenList)
        {
            if (tween.IsValid()) tween.CustomStep(10000.0f);
        }
    }

    public void CustomStep(float step)
    {
        GetLast().CustomStep(step);
    }

    public bool IsValid() 
    {
        foreach (SceneTreeTween tween in tweenList)
        {
            if (tween.IsValid()) return true;
        }
        return false;
    }

    /// <summary>
    /// Invalidates all the tweens in the tween list
    /// </summary>
    /// <param name="_discard"></param>
    public void Reset(object _discard = null)
    {
        foreach (SceneTreeTween tween in tweenList) 
        {
            if (tween.IsValid()) tween.Kill();
        }
    }

}
