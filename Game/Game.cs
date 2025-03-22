// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    //test 123333333333333333333333
    public class Game
    {
        //hi wspppppppppppp
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
                musicTracks[i] = Audio.LoadMusic($"../../../Audio/music{i + 1}.wav");
                tracksPlaying[i] = false; // Initially no tracks are playing
            }

            // Play first track (just an example to start a track)
            //Audio.Play(musicTracks[0]);
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
            if (Input.IsKeyboardKeyPressed(KeyboardInput.One)) // If a button is clicked (for example, the spacebar)
            {
                ToggleTrack(0); // Toggle track 1 (index 0)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Two)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(1); // Toggle track 2 (index 1)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Three)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(2); // Toggle track 3 (index 2)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Four)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(3); // Toggle track 4 (index 3)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Five)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(4); // Toggle track 5 (index 4)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Six)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(5); // Toggle track 6 (index 5)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Seven)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(6); // Toggle track 7 (index 6)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Eight)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(7); // Toggle track 8 (index 7)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Nine)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(8); // Toggle track 9 (index 8)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Zero)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(9); // Toggle track 10 (index 9)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Minus)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(10); // Toggle track 11 (index 10)
            }
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Backspace)) // If a button is clicked (for example, the Enter key)
            {
                ToggleTrack(11); // Toggle track 12 (index 11)
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