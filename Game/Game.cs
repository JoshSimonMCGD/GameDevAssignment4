using System;
using System.Numerics;
using System.Collections.Generic;

namespace MohawkGame2D
{
    public class Game
    {
        private const int numTracks = 18; // Number of music tracks 
        private Music[] musicTracks = new Music[numTracks]; // Music tracks
        private bool[] tracksPlaying = new bool[numTracks]; // Whether each track is playing
        private float[] trackStartTimes = new float[numTracks];// Start times of each track

        private float tempo = 120f;// Beats per minute
        private float secondsPerBeat => 60f / tempo;// Seconds per beat
        private float barDuration => secondsPerBeat * 4f;// Duration of a bar
        private float loopDuration => barDuration * 8f;// Duration of a loop

        private float previousLoopTime = 0f;// Previous loop time
        private float currentLoopTime = 0f;// Current loop time

        private struct QueuedToggle// Queued track toggle
        {
            public int trackIndex;// Track index
            public bool turnOn;// Whether to turn on
        }
        private List<QueuedToggle> queuedToggles = new List<QueuedToggle>(); // Queued track toggles

        public void Setup()
        {
            for (int i = 0; i < numTracks; i++) // Load music tracks
            {
                string filePath = $"../../../Audio/music{i + 1}.wav";// File path
                if (System.IO.File.Exists(filePath))// If file exists
                {
                    musicTracks[i] = Audio.LoadMusic(filePath);// Load music
                    tracksPlaying[i] = false;// Not playing
                }
            }
        }

        public void Update()
        {
            float currentTime = Time.SecondsElapsed;// Current time
            previousLoopTime = currentLoopTime;// Previous loop time
            currentLoopTime = currentTime % loopDuration;// Current loop time

            // If loop just restarted (crossed from last beat to first)
            if (currentLoopTime < previousLoopTime)// Downbeat hit
            {
                Console.WriteLine("Downbeat hit! Playing Toggled ");// Log
                foreach (var toggle in queuedToggles)// Execute queued toggles
                {
                    if (toggle.turnOn)// If turn on
                    {
                        Console.WriteLine($"Starting track {toggle.trackIndex + 1} on downbeat.");
                        Audio.Play(musicTracks[toggle.trackIndex]); // Play track
                        tracksPlaying[toggle.trackIndex] = true; // Set playing
                        trackStartTimes[toggle.trackIndex] = currentTime - currentLoopTime;// Set start time
                    }
                    else 
                    {
     
                        Console.WriteLine($"Stopping track {toggle.trackIndex + 1} on downbeat.");
                        Audio.Stop(musicTracks[toggle.trackIndex]);// Stop track
                        tracksPlaying[toggle.trackIndex] = false;// Set not playing
                    }
                }
                queuedToggles.Clear();// Clear queued toggles
            }

            // Restart loops if ended
            for (int i = 0; i < numTracks; i++)
            {
                if (tracksPlaying[i] && (currentTime - trackStartTimes[i] >= loopDuration))// If track playing and loop ended
                {
                    Audio.Play(musicTracks[i]);
                    trackStartTimes[i] = currentTime - (currentTime % loopDuration);// Set start time
                }
            }

            CheckTrackToggles();// Check for track toggles
        }

        private void CheckTrackToggles()
        {
            (KeyboardInput key, int trackIndex)[] keyMappings = new (KeyboardInput, int)[]// Key mappings
            {
                (KeyboardInput.One, 0), (KeyboardInput.Two, 1), (KeyboardInput.Three, 2),
                (KeyboardInput.Four, 3), (KeyboardInput.Five, 4), (KeyboardInput.Six, 5),
                (KeyboardInput.Seven, 6), (KeyboardInput.Eight, 7), (KeyboardInput.Nine, 8),
                (KeyboardInput.Zero, 9), (KeyboardInput.Minus, 10), (KeyboardInput.Equal, 11),
                (KeyboardInput.Backspace, 12), (KeyboardInput.Tab, 13), (KeyboardInput.Q, 14),
                (KeyboardInput.W, 15), (KeyboardInput.E, 16), (KeyboardInput.R, 17)
            };

            foreach (var (key, trackIndex) in keyMappings)// Check key mappings
            {
                if (Input.IsKeyboardKeyPressed(key))// If key pressed
                {
                    QueueToggleTrack(trackIndex);// Queue track toggle
                }
            }
        }

        private void QueueToggleTrack(int trackIndex)// Queue track toggle
        {
            if (trackIndex < 0 || trackIndex >= numTracks) // If invalid track index     
            {
                Console.WriteLine($"Invalid track index {trackIndex}");
                return;
            }

            bool turnOn = !tracksPlaying[trackIndex];// Whether to turn on
            Console.WriteLine($"Queuing {(turnOn ? "start" : "stop")} for track {trackIndex + 1} at next downbeat.");
            queuedToggles.Add(new QueuedToggle { trackIndex = trackIndex, turnOn = turnOn });// Add to queued toggles
        }
    }
}
