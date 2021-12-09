﻿namespace AdventOfCode2021;

public class Point : IEquatable<Point>
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly int X, Y;
    
    public override string ToString() => $"{X}, {Y}";

    #region IEquatable
    
    public bool Equals(Point? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override bool Equals(object? obj) => Equals(obj as Point);
    
    #endregion
}

public class Grid<T>
{
    public Grid(int width, int height, T invalid)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        Invalid = invalid;
    }

    public void AddRange(IEnumerable<T> tRange) => Cells.AddRange(tRange);
    public T At(int x, int y) => Get(x, y);

    public T Get(int x, int y)
    {
        if (x < 0 || x > Width - 1) return Invalid;
        if (y < 0 || y > Height - 1) return Invalid;

        var idx = y * Width + x;
        return Cells[idx];
    }

    public T At(Point p) => At(p.X, p.Y);

    // doesn't wrap (currently)
    public IEnumerable<T> Neighbours(Point p, bool includeDiagonals = true) => Neighbours(p.X, p.Y, includeDiagonals);
    public IEnumerable<T> Neighbours(int x, int y, bool includeDiagonals = true)
    {
        var neighbours = new List<T>()
        {
            Get(x, y - 1),
            Get(x + 1, y),
            Get(x, y + 1),
            Get(x - 1, y)
        };

        if (includeDiagonals)
        {
            neighbours.Add(Get(x - 1, y - 1));
            neighbours.Add(Get(x + 1, y - 1));
            neighbours.Add(Get(x + 1, y + 1));
            neighbours.Add(Get(x - 1, y + 1));
        }

        return neighbours.Where(n => n != null && !n.Equals(Invalid));
    }

    public IEnumerable<Point> NeighbouringPoints(Point p, bool includeDiagonals = true)
    {
        var neighbours = new List<Point>()
        {
            new (p.X, p.Y - 1),
            new (p.X + 1, p.Y),
            new (p.X, p.Y + 1),
            new (p.X - 1, p.Y)
        };

        if (includeDiagonals)
        {
            neighbours.Add(new Point(p.X - 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y + 1));
            neighbours.Add(new Point(p.X - 1, p.Y + 1));
        }

        return neighbours.Where(ValidPoint);
    }
    
    // checks if the point is within bounds
    public bool ValidPoint(Point point) 
        => point.X >= 0 && point.X <= Width - 1 && point.Y >= 0 && point.Y <= Height - 1;

    private T Invalid;
    private int Width = 0;
    private int Height = 0;
    public List<T> Cells = new List<T>();
}