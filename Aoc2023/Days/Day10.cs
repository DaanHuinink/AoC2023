using System.Drawing;
using System.Drawing.Drawing2D;

namespace Aoc2023.Days;

public sealed class Day10 : IDay
{
    public int PartOne(string input)
    {
        var tileMap = GetTileMap(input);
        StartingPosition startingPosition = GetStartingPosition(tileMap);
        var loop = FindLoop(startingPosition, startingPosition.GetConnectedTiles()[0]);
        return loop.Count / 2 + loop.Count % 2;
    }


    public int PartTwo(string input)
    {
        var tileMap = GetTileMap(input);

        StartingPosition startingPosition = GetStartingPosition(tileMap);

        var loop = FindLoop(startingPosition, startingPosition.GetConnectedTiles()[0]);
        var polygon = loop.Select(l => l.Point).ToArray();

        return tileMap
            .SelectMany(tileRow => tileRow.Where(t => !loop.Contains(t)))
            .Count(tile => IsPointInsidePolygon(polygon, tile.Point));
    }

    private static StartingPosition GetStartingPosition(List<List<Tile>> tileMap)
    {
        StartingPosition startingPosition = tileMap
            .SelectMany(t => t)
            .Select(t => t as StartingPosition)
            .Single(t => t != default);

        return startingPosition;
    }

    private static bool IsPointInsidePolygon(Point[] polygon, Point points)
    {
#pragma warning disable CA1416
        var path = new GraphicsPath();

        path.AddPolygon(polygon);

        var region = new Region(path);
        return region.IsVisible(points);
#pragma warning restore CA1416
    }

    private static List<Tile> FindLoop(
        StartingPosition startingPosition,
        Tile startOfLoop)
    {
        var loop = new List<Tile>();
        Tile previousTile = startingPosition;
        Tile currentTile = startOfLoop;

        loop.Add(currentTile);

        // Iterate until we reach the starting position
        while (currentTile != startingPosition)
        {
            Tile newTile = currentTile.MoveOver(previousTile);
            loop.Add(newTile);
            previousTile = currentTile;
            currentTile = newTile;
        }

        return loop;
    }

    private static List<List<Tile>> GetTileMap(string input)
    {
        var tileTypeMap = ParseTileTypeMap(input);

        var tileMap = new List<List<Tile>>();
        for (var y = 0; y < tileTypeMap.Count; y++)
        {
            var tileRow = new List<Tile>();
            for (var x = 0; x < tileTypeMap[y].Count; x++)
            {
                var tileToAdd = Tile.FromTileType(tileTypeMap[y][x], tileMap, x, y);
                tileRow.Add(tileToAdd);
            }

            tileMap.Add(tileRow);
        }

        return tileMap;
    }

    private static List<List<TileType>> ParseTileTypeMap(string input)
    {
        var tileTypeMap = input
            .ToGrid()
            .Select(s => s.Select(c => (TileType)c).ToList())
            .ToList();

        AddGroundBorder(tileTypeMap);
        return tileTypeMap;
    }

    private static void AddGroundBorder(List<List<TileType>> tileTypeMap)
    {
        var groundRow = Enumerable.Repeat(TileType.Ground, tileTypeMap[0].Count);
        // ReSharper disable PossibleMultipleEnumeration On purpose
        tileTypeMap.Insert(0, groundRow.ToList());
        tileTypeMap.Add(groundRow.ToList());
        // ReSharper restore PossibleMultipleEnumeration

        foreach (var t in tileTypeMap)
        {
            t.Insert(0, TileType.Ground);
            t.Add(TileType.Ground);
        }
    }


    private enum TileType
    {
        VerticalPipe = '|',
        HorizontalPipe = '-',
        NorthToEast = 'L',
        NorthToWest = 'J',
        SouthToWest = '7',
        SouthToEast = 'F',
        Ground = '.',
        StartingPosition = 'S'
    }

    private readonly record struct Position(int X, int Y);

    private abstract class Tile(
        IReadOnlyList<IReadOnlyList<Tile>> tileMap,
        Position position)
    {
        public Point Point => new(Position.X, Position.Y);
        public Position Position { get; } = position;

        protected Tile North => tileMap[Position.Y - 1][Position.X];
        protected Tile South => tileMap[Position.Y + 1][Position.X];
        protected Tile East => tileMap[Position.Y][Position.X + 1];
        protected Tile West => tileMap[Position.Y][Position.X - 1];

        protected static Tile NotFrom(Tile fromTile, Tile tileOne, Tile tileTwo)
            => fromTile == tileOne ? tileTwo : tileOne;

        public abstract Tile MoveOver(Tile fromTile);
        public abstract bool IsConnectedTo(Tile otherTile);

        public static Tile FromTileType(
            TileType type,
            IReadOnlyList<IReadOnlyList<Tile>> tileMap,
            int x,
            int y)
        {
            var position = new Position(x, y);
            return type switch
            {
                TileType.VerticalPipe => new VerticalPipe(tileMap, position),
                TileType.HorizontalPipe => new HorizontalPipe(tileMap, position),
                TileType.NorthToEast => new NorthToEast(tileMap, position),
                TileType.NorthToWest => new NorthToWest(tileMap, position),
                TileType.SouthToWest => new SouthToWest(tileMap, position),
                TileType.SouthToEast => new SouthToEast(tileMap, position),
                TileType.Ground => new Ground(tileMap, position),
                TileType.StartingPosition => new StartingPosition(tileMap, position),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }


    private sealed class VerticalPipe(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, North, South);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == North || otherTile == South;
    }

    private sealed class HorizontalPipe(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, East, West);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == East || otherTile == West;
    }

    private sealed class NorthToEast(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, North, East);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == North || otherTile == East;
    }

    private sealed class NorthToWest(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, North, West);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == North || otherTile == West;
    }

    private sealed class SouthToWest(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, South, West);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == South || otherTile == West;
    }

    private sealed class SouthToEast(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => NotFrom(fromTile, South, East);
        public override bool IsConnectedTo(Tile otherTile) => otherTile == South || otherTile == East;
    }

    private sealed class Ground(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile) => throw new InvalidOperationException("Cannot move over ground");
        public override bool IsConnectedTo(Tile otherTile) => false;
    }

    private sealed class StartingPosition(IReadOnlyList<IReadOnlyList<Tile>> tileMap, Position position)
        : Tile(tileMap, position)
    {
        public override Tile MoveOver(Tile fromTile)
        {
            throw new NotSupportedException("Can't move over starting position");
        }

        public override bool IsConnectedTo(Tile otherTile)
        {
            throw new NotSupportedException("Cannot determine if starting position is connected");
        }

        public IReadOnlyList<Tile> GetConnectedTiles()
        {
            return new[] { North, East, South, West }
                .Where(t => t.IsConnectedTo(this))
                .ToList();
        }
    }
}