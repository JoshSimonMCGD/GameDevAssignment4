using System;
using System.Numerics;
using System.Collections.Generic;

namespace MohawkGame2D
{
    public class Game
    {
        private const int numTracks = 18;// Number of music tracks
        private Music[] musicTracks = new Music[numTracks];// Array of music tracks
        private bool[] tracksPlaying = new bool[numTracks];// Array of flags for each track
        private float[] trackStartTimes = new float[numTracks];// Array of start times for each track
        private List<QueuedToggle> queuedToggles = new List<QueuedToggle>();// List of queued toggles

        private float tempo = 120f;// Tempo of the music
        private float secondsPerBeat => 60f / tempo;// Duration of a beat in seconds
        private float barDuration => secondsPerBeat * 4f;// Duration of a bar in seconds
        private float loopDuration => barDuration * 8f;// Duration of a loop in seconds
        private float previousLoopTime = 0f;// Previous loop time in seconds
        private float currentLoopTime = 0f;// Current loop time in seconds

        private struct QueuedToggle// Struct for queued toggles
        {
            public int trackIndex;// Index of the track
            public bool turnOn;// Flag to turn on or off
        }
        
        private Texture2D Background = Graphics.LoadTexture("../../../VisualAssets/BackgroundCave.png");// Background texture
        private Texture2D Shrine = Graphics.LoadTexture("../../../VisualAssets/BaseShrine.png");// Shrine texture
        private Texture2D[] assets;// Array of asset textures
        private Vector2[] assetPositions;// Array of asset positions
        private bool[] hidden;// Array of hidden flags

        public void Setup()
        {
            Window.SetTitle("Shadow Wizard Money Gang Mixer");
            Window.SetSize(800, 600);
            hidden = new bool[numTracks];

            assets = new Texture2D[numTracks] {
                Graphics.LoadTexture("../../../VisualAssets/PurpleTallGem.png"),// Load all asset textures
                Graphics.LoadTexture("../../../VisualAssets/OrangeTallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/RedTallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/GreenTallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/BlueTallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/GreenMediumGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/RedMediumGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/GreenSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/BlueSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/DarkBlueSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/VioletSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/PurpleSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/OrangeSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/PinkSmallGem.png"),
                Graphics.LoadTexture("../../../VisualAssets/BlueNode.png"),
                Graphics.LoadTexture("../../../VisualAssets/PurpleNode.png"),
                Graphics.LoadTexture("../../../VisualAssets/VioletNode.png"),
                Graphics.LoadTexture("../../../VisualAssets/OrangeNode.png")
            };

            assetPositions = new Vector2[numTracks] {
                new Vector2(308, 114), new Vector2(375, 136), new Vector2(435, 189),// Set all asset positions
                new Vector2(182, 182), new Vector2(245, 138), new Vector2(139, 245),
                new Vector2(491, 240), new Vector2(145, 362), new Vector2(200, 370),
                new Vector2(264, 379), new Vector2(326, 365), new Vector2(355, 375),
                new Vector2(433, 373), new Vector2(500, 366), new Vector2(114, 447),
                new Vector2(218, 459), new Vector2(316, 453), new Vector2(447, 455)
            };

            for (int i = 0; i < numTracks; i++)
            {
                string filePath = $"../../../Audio/music{i + 1}.wav";// Load all music tracks
                if (System.IO.File.Exists(filePath))// Check if the file exists
                {
                    musicTracks[i] = Audio.LoadMusic(filePath);// Load the music track
                    tracksPlaying[i] = false;// Set the track to not playing
                }
            }
        }

        public void Update()// Update method
        {
            float currentTime = Time.SecondsElapsed;// Get the current time in seconds
            previousLoopTime = currentLoopTime;// Set the previous loop time to the current loop time
            currentLoopTime = currentTime % loopDuration;// Set the current loop time to the current time modulo the loop duration

            if (currentLoopTime < previousLoopTime)// Check if the current loop time is less than the previous loop time
            {
                foreach (var toggle in queuedToggles)// Loop through each queued toggle
                {
                    if (toggle.turnOn)// Check if the toggle is on
                    {
                        Audio.Play(musicTracks[toggle.trackIndex]);// Play the music track
                        tracksPlaying[toggle.trackIndex] = true;// Set the track to playing
                        trackStartTimes[toggle.trackIndex] = currentTime - currentLoopTime;// Set the track start time
                    }
                    else// If the toggle is off
                    {
                        Audio.Stop(musicTracks[toggle.trackIndex]);   // Stop the music track
                        tracksPlaying[toggle.trackIndex] = false;// Set the track to not playing
                    }
                }
                queuedToggles.Clear();// Clear the queued toggles
            }

            for (int i = 0; i < numTracks; i++)// Loop through each track
            {
                if (tracksPlaying[i] && (currentTime - trackStartTimes[i] >= loopDuration))// Check if the track is playing and the current time minus the track start time is greater than or equal to the loop duration
                {
                    Audio.Play(musicTracks[i]);// Play the music track
                    trackStartTimes[i] = currentTime - (currentTime % loopDuration);// Set the track start time
                }
            }

            CheckMouseInput();// Check the mouse input
            DrawGraphics();// Draw the graphics
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
        }

        private void CheckMouseInput()
        {
            Vector2 MousePosition = new Vector2(Input.GetMouseX(), Input.GetMouseY());// Get the mouse position

            bool IsMouseOver(Vector2 pos, Texture2D texture) =>// Function to check if the mouse is over a texture
                MousePosition.X >= pos.X && MousePosition.X <= pos.X + texture.Width &&// Check if the mouse is over the texture
                MousePosition.Y >= pos.Y && MousePosition.Y <= pos.Y + texture.Height;// Check if the mouse is over the texture

            if (Input.IsMouseButtonPressed(MouseInput.Left))// Check if the left mouse button is pressed
            {
                for (int i = 0; i < assets.Length; i++)// Loop through each asset
                {
                    if (IsMouseOver(assetPositions[i], assets[i]))// Check if the mouse is over the asset
                    {
                        hidden[i] = !hidden[i];// Toggle the hidden flag
                        QueueToggleTrack(i);// Queue the toggle track
                    }
                }
            }
        }

        private void QueueToggleTrack(int trackIndex)// Method to queue a toggle track
        {
            if (trackIndex < 0 || trackIndex >= numTracks) return;// Check if the track index is out of bounds
            bool turnOn = !tracksPlaying[trackIndex];// Set the turn on flag
            queuedToggles.Add(new QueuedToggle { trackIndex = trackIndex, turnOn = turnOn });// Add the queued toggle
        }

        private void DrawGraphics()// Method to draw the graphics
        {
            ColorF NormTint = new ColorF(1.0f, 1.0f, 1.0f);// Set the normal tint color
            ColorF OffTint = new ColorF(0.5f, 0.5f, 0.5f);// Set the off tint color

            Graphics.Tint = NormTint;// Set the tint color
            Graphics.Draw(Background, 0, 0);// Draw the background
            Graphics.Draw(Shrine, 0, 0);// Draw the shrine

            Graphics.Tint = OffTint;// Set the off tint color
            for (int i = 0; i < assets.Length; i++)// Loop through each asset
            {
                if (!hidden[i])// Check if the asset is not hidden
                {
                    Graphics.Draw(assets[i], assetPositions[i].X, assetPositions[i].Y);// Draw the asset
                }
            }
        }
    }
}