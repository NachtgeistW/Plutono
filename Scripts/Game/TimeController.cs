using Godot;
using System;

namespace Plutono.Scripts.Game
{
    public partial class TimeController : Node3D
    {
        [Export] AudioStreamPlayer player;

        public double CurTime { get; private set; }
        private double StartOrResumeTime { get; set; }
        private double ResumeElapsedTime { get; set; }
        private double musicStartTime;
        private double musicPlayingDelay;
        private double lastDspTime = -1;
        private double curDspTime = -1;
        private int ticksBeforeSynchronization = 600;

        private double configChartMusicOffset;
        private float configGlobalChartOffset;

        public override void _Ready()
        {
            base._Ready();

            curDspTime = player.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix() - AudioServer.GetOutputLatency();
            musicStartTime = curDspTime;
            player.Play();

            StartOrResumeTime = Time.GetTicksUsec();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            SynchronizeTime(delta);
        }

        private void SynchronizeTime(double delta)
        {
            ticksBeforeSynchronization--;
            ResumeElapsedTime = (Time.GetTicksUsec() - StartOrResumeTime) / 1000000.0d;
            curDspTime = player.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix() - AudioServer.GetOutputLatency();

            // Sync: every 600 ticks (=10 seconds) and every tick within the first 0.5 seconds after start/resume
            if ((ticksBeforeSynchronization <= 0 || ResumeElapsedTime < 0.5f)
                && Math.Abs(lastDspTime - curDspTime) > 0.001)
            {
                ticksBeforeSynchronization = 600;
                lastDspTime = curDspTime;
                CurTime = curDspTime -musicStartTime + configGlobalChartOffset + configChartMusicOffset;
            }
            else
            {
                CurTime += delta;
            }
        }

    }
}
