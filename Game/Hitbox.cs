using System;
using System.Numerics;

namespace MohawkGame2D
{
    public class Hitbox
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public Hitbox(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool IsClicked(Vector2 mousePosition, bool mousePressed)
        {
            if (mousePressed &&
                mousePosition.X >= X &&
                mousePosition.X <= X + Width &&
                mousePosition.Y >= Y &&
                mousePosition.Y <= Y + Height)
            {
                return true;
            }
            return false;
        }
    }
}
