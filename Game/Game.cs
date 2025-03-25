using System;
using System.Numerics;
using System.Collections.Generic;

namespace MohawkGame2D
{
    public class Game
    {
        private const int numTracks = 18;
        private Music[] musicTracks = new Music[numTracks];
        private bool[] tracksPlaying = new bool[numTracks];
        private float[] trackStartTimes = new float[numTracks];

        private const int numTracks = 12; // Number of tracks
        private Music[] musicTracks = new Music[numTracks]; // Array to hold the 12 music tracks
        private bool[] tracksPlaying = new bool[numTracks]; // To track whether each track is playing
        private float[] trackStartTimes = new float[numTracks]; // Track when each loop should start

        private float tempo = 120f;
        private float secondsPerBeat => 60f / tempo;
        private float barDuration => secondsPerBeat * 4f;
        private float loopDuration => barDuration * 8f;

        private float previousLoopTime = 0f;
        private float currentLoopTime = 0f;

        private struct QueuedToggle
        {
            public int trackIndex;
            public bool turnOn;
        }
        private List<QueuedToggle> queuedToggles = new List<QueuedToggle>();

        public void Setup()
        {
            for (int i = 0; i < numTracks; i++)
            {
                string filePath = $"../../../Audio/music{i + 1}.wav";
                if (System.IO.File.Exists(filePath))
                {
                    musicTracks[i] = Audio.LoadMusic(filePath);
                    tracksPlaying[i] = false;
                }
            }
        }

        public void Update()
        {
            float currentTime = Time.SecondsElapsed;
            previousLoopTime = currentLoopTime;
            currentLoopTime = currentTime % loopDuration;

            // If loop just restarted (crossed from high value to low)
            if (currentLoopTime < previousLoopTime)
            {
                Console.WriteLine("Downbeat hit! Executing queued toggles.");
                foreach (var toggle in queuedToggles)
                {
                    if (toggle.turnOn)
                    {
                        Console.WriteLine($"Starting track {toggle.trackIndex + 1} on downbeat.");
                        Audio.Play(musicTracks[toggle.trackIndex]);
                        tracksPlaying[toggle.trackIndex] = true;
                        trackStartTimes[toggle.trackIndex] = currentTime - currentLoopTime;
                    }
                    else
                    {
                        Console.WriteLine($"Stopping track {toggle.trackIndex + 1} on downbeat.");
                        Audio.Stop(musicTracks[toggle.trackIndex]);
                        tracksPlaying[toggle.trackIndex] = false;
                    }
                }
                queuedToggles.Clear();
            }

            // Restart loops if ended
            for (int i = 0; i < numTracks; i++)
            {
                if (tracksPlaying[i] && (currentTime - trackStartTimes[i] >= loopDuration))
                {
                    Audio.Play(musicTracks[i]);
                    trackStartTimes[i] = currentTime - (currentTime % loopDuration);
                }
            }

            CheckTrackToggles();
        }

        private void CheckTrackToggles()
        {
            (KeyboardInput key, int trackIndex)[] keyMappings = new (KeyboardInput, int)[]
            {
                (KeyboardInput.One, 0), (KeyboardInput.Two, 1), (KeyboardInput.Three, 2),
                (KeyboardInput.Four, 3), (KeyboardInput.Five, 4), (KeyboardInput.Six, 5),
                (KeyboardInput.Seven, 6), (KeyboardInput.Eight, 7), (KeyboardInput.Nine, 8),
                (KeyboardInput.Zero, 9), (KeyboardInput.Minus, 10), (KeyboardInput.Equal, 11),
                (KeyboardInput.Backspace, 12), (KeyboardInput.Tab, 13), (KeyboardInput.Q, 14),
                (KeyboardInput.W, 15), (KeyboardInput.E, 16), (KeyboardInput.R, 17)
            };

            foreach (var (key, trackIndex) in keyMappings)
            {
                if (Input.IsKeyboardKeyPressed(key))
                {
                    QueueToggleTrack(trackIndex);
                }
            }
        }

        private void QueueToggleTrack(int trackIndex)
        {
            if (trackIndex < 0 || trackIndex >= numTracks)
            {
                Console.WriteLine($"Invalid track index {trackIndex}");
                return;
            }

            bool turnOn = !tracksPlaying[trackIndex];
            Console.WriteLine($"Queuing {(turnOn ? "start" : "stop")} for track {trackIndex + 1} at next downbeat.");
            queuedToggles.Add(new QueuedToggle { trackIndex = trackIndex, turnOn = turnOn });
        }
    }
}
