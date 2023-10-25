using Arch.Core;
using Catalyster.Components;
using System.Runtime.CompilerServices;
using Trial;

public struct TokenUpdate : IForEach<Position, Token>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update(ref Position position, ref Token token)
    {
        Program.MapConsole.SetGlyph(position.X, position.Y, token.Char);
        Program.MapConsole.SetForeground(position.X, position.Y, new Color(token.Color));
    }
}