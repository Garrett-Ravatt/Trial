using Catalyster.Interfaces;
using Catalyster.Core;
using RogueSharp.Random;

namespace Catalyster.Models
{
    public class InitializeMap : IStep<DungeonMap>
    {
        private int Width;
        private int Height;

        public InitializeMap(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public DungeonMap Step(DungeonMap subject, int _seed)
        {
            subject.Initialize(Width, Height);
            return subject;
        }
    }
}
