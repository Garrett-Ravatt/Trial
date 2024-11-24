namespace Catalyster.Components
{
    // Coordinates on the map
    public struct Position {
        public int X, Y;
        public override string ToString() { return $"({X}, {Y})"; }
    };

    // Representation in Console
    public struct Token { public string RID; public string Name; public char Char; public uint Color; };

    // Ability to detect creatures. Later will support blindsight, tremorsense, etc
    public struct Sense { public int Range; };
}
