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
        private List<QueuedToggle> queuedToggles = new List<QueuedToggle>();

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

        private Texture2D Background = Graphics.LoadTexture("../../../VisualAssets/BackgroundCave.png");
        private Texture2D Shrine = Graphics.LoadTexture("../../../VisualAssets/BaseShrine.png");
        private Texture2D[] assets;
        private Vector2[] assetPositions;
        private bool[] hidden;

        public void Setup()
        {
            Window.SetTitle("Shadow Wizard Money Gang Mixer");
            Window.SetSize(800, 600);
            hidden = new bool[numTracks];

            assets = new Texture2D[numTracks] {
                Graphics.LoadTexture("../../../VisualAssets/PurpleTallGem.png"),
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
                new Vector2(308, 114), new Vector2(375, 136), new Vector2(435, 189),
                new Vector2(182, 182), new Vector2(245, 138), new Vector2(139, 245),
                new Vector2(491, 240), new Vector2(145, 362), new Vector2(200, 370),
                new Vector2(264, 379), new Vector2(326, 365), new Vector2(355, 375),
                new Vector2(433, 373), new Vector2(500, 366), new Vector2(114, 447),
                new Vector2(218, 459), new Vector2(316, 453), new Vector2(447, 455)
            };

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

            if (currentLoopTime < previousLoopTime)
            {
                foreach (var toggle in queuedToggles)
                {
                    if (toggle.turnOn)
                    {
                        Audio.Play(musicTracks[toggle.trackIndex]);
                        tracksPlaying[toggle.trackIndex] = true;
                        trackStartTimes[toggle.trackIndex] = currentTime - currentLoopTime;
                    }
                    else
                    {
                        Audio.Stop(musicTracks[toggle.trackIndex]);
                        tracksPlaying[toggle.trackIndex] = false;
                    }
                }
                queuedToggles.Clear();
            }

            for (int i = 0; i < numTracks; i++)
            {
                if (tracksPlaying[i] && (currentTime - trackStartTimes[i] >= loopDuration))
                {
                    Audio.Play(musicTracks[i]);
                    trackStartTimes[i] = currentTime - (currentTime % loopDuration);
                }
            }

            CheckMouseInput();
            DrawGraphics();
        }

        private void CheckMouseInput()
        {
            Vector2 MousePosition = new Vector2(Input.GetMouseX(), Input.GetMouseY());

            bool IsMouseOver(Vector2 pos, Texture2D texture) =>
                MousePosition.X >= pos.X && MousePosition.X <= pos.X + texture.Width &&
                MousePosition.Y >= pos.Y && MousePosition.Y <= pos.Y + texture.Height;

            if (Input.IsMouseButtonPressed(MouseInput.Left))
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    if (IsMouseOver(assetPositions[i], assets[i]))
                    {
                        hidden[i] = !hidden[i];
                        QueueToggleTrack(i);
                    }
                }
            }
        }

        private void QueueToggleTrack(int trackIndex)
        {
            if (trackIndex < 0 || trackIndex >= numTracks) return;
            bool turnOn = !tracksPlaying[trackIndex];
            queuedToggles.Add(new QueuedToggle { trackIndex = trackIndex, turnOn = turnOn });
        }

        private void DrawGraphics()
        {
            ColorF NormTint = new ColorF(1.0f, 1.0f, 1.0f);
            ColorF OffTint = new ColorF(0.5f, 0.5f, 0.5f);

            Graphics.Tint = NormTint;
            Graphics.Draw(Background, 0, 0);
            Graphics.Draw(Shrine, 0, 0);

            Graphics.Tint = OffTint;
            for (int i = 0; i < assets.Length; i++)
            {
                if (!hidden[i])
                {
                    Graphics.Draw(assets[i], assetPositions[i].X, assetPositions[i].Y);
                }
            }
        }
    }
}