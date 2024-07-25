using System;
using System.Globalization;
using Godot;
using Plutono.Scripts.Utils;

namespace Plutono.Scripts.Game;

public partial class Game : Node3D
{
    [Export] public TimeController TimeControl { get; set; }

    [Export] public Camera3D OrthographicCamera { get; set; }
    [Export] public RichTextLabel CurTimeText { get; set; }

    public float ChartPlaySpeed { get; internal set; } = 5f;
    public GameMode Mode { get; private set; } = GameMode.Floro;


    public override void _Ready()
    {
        base._Ready();
   }

    public override void _Process(double delta)
    {
        base._Process(delta);

        CurTimeText.Text = TimeControl.CurTime.ToString(CultureInfo.InvariantCulture);
    }


    //private void SynchronizeTime(double delta)
    //{
    //        ticksBeforeSynchronization--;
    //        ResumeElapsedTime = Time.GetTicksUsec() - StartOrResumeTime;
    //        curDspTime = AudioSettings.dspTime;
    //        // Sync: every 600 ticks (=10 seconds) and every tick within the first 0.5 seconds after start/resume
    //        if ((ticksBeforeSynchronization <= 0 || ResumeElapsedTime < 0.5f)
    //            && Math.Abs(lastDspTime - curDspTime) > 0.001)
    //        {
    //            ticksBeforeSynchronization = 600;
    //            lastDspTime = curDspTime;
    //            CurTime = (float)curDspTime - musicStartTime - configGlobalChartOffset + configChartMusicOffset;
    //#if DEBUG
    //                Debug.Log("--SynchronizeTime--\nStarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime
    //                    + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
    //#endif
    //        }
    //        else
    //        {
    //            CurTime += delta;
    //        }

    //CurTime = (Time.GetTicksUsec() - timeBegin) / 1000000.0d;
    //CurTime = Math.Max(0.0d, CurTime - timeDelay);

    //    CurTime = GetNode<AudioStreamPlayer>("Player").GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
    //    // Compensate for output latency.
    //    CurTime -= AudioServer.GetOutputLatency();
    //}

}
