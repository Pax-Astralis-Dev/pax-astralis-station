using JetBrains.Annotations;


namespace Content.Server._PAX.Maps;

[DataDefinition, PublicAPI]
public sealed partial class Location
{
    public Location()
    {
        Distance = null;
    }

    public Location(int distance)
    {
        Distance = distance;
    }

    public Location(Position position)
    {
        Position = position;
    }

    [DataField("distance")]
    public int? Distance { get; private set; }

    [DataField("position")]
    public Position? Position { get; private set; }
}

[DataDefinition, PublicAPI]
public sealed partial class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    [DataField("x", required: true)]
    public int X { get; private set; }

    [DataField("y", required: true)]
    public int Y { get; private set; }
}

[DataDefinition, PublicAPI]
public sealed partial class Color
{
    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public byte R { get; } = 0;

    public byte G { get; } = 0;

    public byte B { get; } = 0;
}
