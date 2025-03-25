// Include necessary namespaces
using System;
using System.Numerics;

namespace MohawkGame2D
{
    public class Game
    {
        private const int numTracks = 18; // Set Array to hold 18 tracks
        private Music[] musicTracks = new Music[numTracks]; // Array to hold music tracks
        private bool[] tracksPlaying = new bool[numTracks]; // Track playing states
        private float[] trackStartTimes = new float[numTracks]; // Track loop start times

        private float tempo = 120f; // BPM
        private float secondsPerBeat => 60f / tempo; // Time per beat in seconds
        private float barDuration => secondsPerBeat * 4f; // 1 bar (4 beats)
        private float loopDuration => barDuration * 8f; // 8-bar loop duration

        private float lastBeatTime = 0f; // Tracks last beat for syncing

        public void Setup()
        {
            // Load all 18 tracks 
            for (int i = 0; i < numTracks; i++)
            {
                string filePath = $"../../../Audio/music{i + 1}.wav";
                if (System.IO.File.Exists(filePath))
                {
                    musicTracks[i] = Audio.LoadMusic(filePath);
                    tracksPlaying[i] = false; // No tracks play to start
                }
                
            }
        }

        public void Update()
        {
            // Get elapsed time
            float currentTime = Time.SecondsElapsed;

            // Sync track loops to start on the correct beat
            if (currentTime - lastBeatTime >= barDuration)
            {
                lastBeatTime += barDuration; // Move to the next beat/bar
            }

            // Restart tracks if their loop has ended
            for (int i = 0; i < numTracks; i++)
            {
                if (tracksPlaying[i] && (currentTime - trackStartTimes[i] >= loopDuration))
                {
                    Audio.Play(musicTracks[i]);
                    trackStartTimes[i] = lastBeatTime; // Resync to the next beat
                }
            }

            // Check keyboard input to toggle tracks
            CheckTrackToggles();
        }

        private void CheckTrackToggles()
        {
            // Map keys to track indexes
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
                    ToggleTrack(trackIndex);
                }
            }
        }

        private void ToggleTrack(int trackIndex)
        {
            if (trackIndex < 0 || trackIndex >= numTracks)
            {
                Console.WriteLine($"Error: Invalid track index {trackIndex}");
                return;
            }

            if (!tracksPlaying[trackIndex])
            {
                // Start playing the track in sync with the beat
                Console.WriteLine($"Starting track {trackIndex + 1}");
                Audio.Play(musicTracks[trackIndex]);
                tracksPlaying[trackIndex] = true;
                trackStartTimes[trackIndex] = lastBeatTime;
            }
            else
            {
                // Stop the track
                Console.WriteLine($"Stopping track {trackIndex + 1}");
                Audio.Stop(musicTracks[trackIndex]);
                tracksPlaying[trackIndex] = false;
            }
        }
    }
}
