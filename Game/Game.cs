﻿// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    //test 123333333333333333333333
    public class Game
    {
        private const int numTracks = 12; // Number of tracks
        private Music[] musicTracks = new Music[numTracks]; // Array to hold the 12 music tracks
        private bool[] tracksPlaying = new bool[numTracks]; // To track whether each track is playing
        private float[] trackStartTimes = new float[numTracks]; // Track when each loop should start

        private float tempo = 120f; // BPM
        private float secondsPerBeat => 60f / tempo; // Time per beat in seconds
        private float barDuration => secondsPerBeat * 4f; // Duration of 1 bar (4 beats)
        private float loopDuration => barDuration * 8f; // Duration of 8 bars (one loop)

        private float lastBeatTime = 0f; // Track the time of the last beat to sync loops

        public void Setup()
        {
            // Load your 12 music tracks (adjust file paths as needed)
            for (int i = 0; i < numTracks; i++)
            {
                musicTracks[i] = Audio.LoadMusic($"../../../Assets/music{i + 1}.wav");
                tracksPlaying[i] = false; // Initially no tracks are playing
            }

            // Play first track (just an example to start a track)
            Audio.Play(musicTracks[0]);
        }

        public void Update()
        {
            // Get the current time in seconds
            float currentTime = Time.SecondsElapsed;

            // Synchronize all tracks so that they start at the right time (on the 1st beat of a bar)
            if (currentTime - lastBeatTime >= barDuration)
            {
                lastBeatTime += barDuration; // Move to the next beat/bar
            }

            // Update the music tracks and play them at the correct time
            for (int i = 0; i < numTracks; i++)
            {
                // Check if the track is supposed to start or stop
                if (tracksPlaying[i])
                {
                    if (currentTime - trackStartTimes[i] >= loopDuration)
                    {
                        // The loop has finished, so restart it to keep it in sync
                        Audio.Play(musicTracks[i]);
                        trackStartTimes[i] = lastBeatTime; // Resync to the next beat
                    }
                }
            }

            // Handle button presses (these are just examples, implement actual button logic)
            if (Input.IsKeyboardKeyDown(KeyboardInput.Space)) // If a button is clicked (for example, the spacebar)
            {
                ToggleTrack(0); // Toggle track 1 (index 0)
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.Enter)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(1); // Toggle track 2 (index 1)
            }
        }

        // Method to toggle a track (start it if it's not playing, stop it if it's playing)
        private void ToggleTrack(int trackIndex)
        {
            if (!tracksPlaying[trackIndex])
            {
                // Start the track at the current beat
                Audio.Play(musicTracks[trackIndex]);
                tracksPlaying[trackIndex] = true;
                trackStartTimes[trackIndex] = lastBeatTime; // Sync to the current beat
            }
            else
            {
                // Stop the track if it's already playing
                Audio.Stop(musicTracks[trackIndex]);
                tracksPlaying[trackIndex] = false;
            }
        }
    }
}