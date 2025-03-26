// Include the namespaces (code libraries) you need below.
using System;
using System.Dynamic;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
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
        
        Texture2D Background = Graphics.LoadTexture("../../../VisualAssets/BackgroundCave.png");
        Texture2D Shrine = Graphics.LoadTexture("../../../VisualAssets/BaseShrine.png");
        
        Texture2D PurpleTallGem = Graphics.LoadTexture("../../../VisualAssets/PurpleTallGem.png");
        Texture2D OrangeTallGem = Graphics.LoadTexture("../../../VisualAssets/OrangeTallGem.png");
        Texture2D RedTallGem = Graphics.LoadTexture("../../../VisualAssets/RedTallGem.png");
        Texture2D GreenTallGem = Graphics.LoadTexture("../../../VisualAssets/GreenTallGem.png");
        Texture2D BlueTallGem = Graphics.LoadTexture("../../../VisualAssets/BlueTallGem.png");
        Texture2D RedMediumGem = Graphics.LoadTexture("../../../VisualAssets/RedMediumGem.png");
        Texture2D GreenMediumGem = Graphics.LoadTexture("../../../VisualAssets/GreenMediumGem.png");
        
        Texture2D GreenSmallGem = Graphics.LoadTexture("../../../VisualAssets/GreenSmallGem.png");
        Texture2D BlueSmallGem = Graphics.LoadTexture("../../../VisualAssets/BlueSmallGem.png");
        Texture2D DarkBlueSmallGem = Graphics.LoadTexture("../../../VisualAssets/DarkBlueSmallGem.png");
        Texture2D VioletSmallGem = Graphics.LoadTexture("../../../VisualAssets/VioletSmallGem.png");
        Texture2D PurpleSmallGem = Graphics.LoadTexture("../../../VisualAssets/PurpleSmallGem.png");
        Texture2D OrangeSmallGem = Graphics.LoadTexture("../../../VisualAssets/OrangeSmallGem.png");
        Texture2D PinkSmallGem = Graphics.LoadTexture("../../../VisualAssets/PinkSmallGem.png");
        
        Texture2D BlueNode = Graphics.LoadTexture("../../../VisualAssets/BlueNode.png");
        Texture2D PurpleNode = Graphics.LoadTexture("../../../VisualAssets/PurpleNode.png");
        Texture2D VioletNode = Graphics.LoadTexture("../../../VisualAssets/VioletNode.png");
        Texture2D OrangeNode = Graphics.LoadTexture("../../../VisualAssets/OrangeNode.png");
        Texture2D RedNode = Graphics.LoadTexture("../../../VisualAssets/RedNode.png");
        
        Texture2D WizardArm = Graphics.LoadTexture("../../../VisualAssets/WizardArm.png");
        
        // Track visibility of assets
        private bool[] hidden;

        public void Setup()
        {
            Window.SetTitle("Shadow Wizard Money Gang Mixer");
            Window.SetSize(800, 600);
            
            hidden = new bool[19]; // Make sure the size matches the number of assets
            
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
            float r = 1.0f;
            float g = 1.0f;
            float b = 1.0f;
            
            float r2 = 0.5f;
            float g2 = 0.5f;
            float b2 = 0.5f;
            
            ColorF NormTint = new ColorF(r, g, b);
            ColorF OffTint = new ColorF(r2, g2, b2);
            
            Vector2 MousePosition = new Vector2(Input.GetMouseX(), Input.GetMouseY());
            
            // List of assets with their positions
            Texture2D[] assets = {
                PurpleTallGem, OrangeTallGem, RedTallGem, GreenTallGem, BlueTallGem, 
                GreenMediumGem, RedMediumGem, GreenSmallGem, BlueSmallGem, DarkBlueSmallGem, 
                VioletSmallGem, PurpleSmallGem, OrangeSmallGem, PinkSmallGem, BlueNode, 
                PurpleNode, VioletNode, OrangeNode, RedNode
            };

            Vector2[] assetPositions = {
                new Vector2(308, 114), new Vector2(375, 136), new Vector2(435, 189),
                new Vector2(182, 182), new Vector2(245, 138), new Vector2(139, 245),
                new Vector2(491, 240), new Vector2(145, 362), new Vector2(200, 370),
                new Vector2(264, 379), new Vector2(326, 365), new Vector2(355, 375),
                new Vector2(433, 373), new Vector2(500, 366), new Vector2(114, 447),
                new Vector2(218, 459), new Vector2(316, 453), new Vector2(447, 455),
                new Vector2(534, 443)
            };

            // Function to check if mouse is over a texture
            bool IsMouseOver(Vector2 pos, Texture2D texture)
            {
                return MousePosition.X >= pos.X && MousePosition.X <= pos.X + texture.Width &&
                       MousePosition.Y >= pos.Y && MousePosition.Y <= pos.Y + texture.Height;
            }

            // Handle mouse click to toggle asset visibility
            if (Input.IsMouseButtonPressed(MouseInput.Left))
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    if (IsMouseOver(assetPositions[i], assets[i]))
                    {
                        hidden[i] = !hidden[i]; // Toggle visibility
                    }
                }
            }

            // Draw background and shrine (always visible)
            Graphics.Tint = NormTint;
            Graphics.Draw(Background, 0, 0);
            Graphics.Draw(Shrine, 0, 0);

            // Draw assets only if they are not hidden and not hovered
            Graphics.Tint = OffTint;
            for (int i = 0; i < assets.Length; i++)
            {
                if (!hidden[i] && !IsMouseOver(assetPositions[i], assets[i]))
                {
                    Graphics.Draw(assets[i], assetPositions[i].X, assetPositions[i].Y);
                }
            }
            
            
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