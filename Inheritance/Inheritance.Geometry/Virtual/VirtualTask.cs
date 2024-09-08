namespace Inheritance.Geometry.Virtual;

public abstract class Body
{
    public Vector3 Position { get; }

    protected Body(Vector3 position)
    {
        Position = position;
    }

    public abstract bool ContainsPoint(Vector3 point);

    public abstract RectangularCuboid GetBoundingBox();
}

public class Ball : Body
{
    public double Radius { get; }

    public Ball(Vector3 position, double radius) : base(position)
    {
        Radius = radius;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        var vector = point - Position;
        var length2 = vector.GetLength2();
        return length2 <= Radius * Radius;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        return new RectangularCuboid(
            Position,
            2 * Radius,
            2 * Radius,
            2 * Radius
        );
    }
}

public class RectangularCuboid : Body
{
    public double SizeX { get; }
    public double SizeY { get; }
    public double SizeZ { get; }

    public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
    {
        SizeX = sizeX;
        SizeY = sizeY;
        SizeZ = sizeZ;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        var minPoint = new Vector3(
            Position.X - SizeX / 2,
            Position.Y - SizeY / 2,
            Position.Z - SizeZ / 2);
        var maxPoint = new Vector3(
            Position.X + SizeX / 2,
            Position.Y + SizeY / 2,
            Position.Z + SizeZ / 2);

        return point >= minPoint && point <= maxPoint;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        return new RectangularCuboid(
            Position,
            SizeX,
            SizeY,
            SizeZ
        );
    }
}

public class Cylinder : Body
{
    public double SizeZ { get; }
    public double Radius { get; }

    public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
    {
        SizeZ = sizeZ;
        Radius = radius;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        var vectorX = point.X - Position.X;
        var vectorY = point.Y - Position.Y;
        var length2 = vectorX * vectorX + vectorY * vectorY;
        var minZ = Position.Z - SizeZ / 2;
        var maxZ = minZ + SizeZ;

        return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        return new RectangularCuboid(
            Position,
            2 * Radius,
            2 * Radius,
            SizeZ
        );
    }
}

public class CompoundBody : Body
{
    public IReadOnlyList<Body> Parts { get; }

    public CompoundBody(IReadOnlyList<Body> parts) : base(new Vector3(
        parts.Average(body => body.Position.X),
        parts.Average(body => body.Position.Y),
        parts.Average(body => body.Position.Z)
    ))
    {
        Parts = parts;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        return Parts.Any(body => body.ContainsPoint(point));
    }

    public override RectangularCuboid GetBoundingBox()
    {
        var rectangles = Parts
            .Select(part => part.GetBoundingBox())
            .ToArray();
        var minX = rectangles
            .Min(body => body.Position.X - body.SizeX / 2);
        var minY = rectangles
            .Min(body => body.Position.Y - body.SizeY / 2);
        var minZ = rectangles
            .Min(body => body.Position.Z - body.SizeZ / 2);

        var maxX = rectangles
            .Max(body => body.Position.X + body.SizeX / 2);
        var maxY = rectangles
            .Max(body => body.Position.Y + body.SizeY / 2);
        var maxZ = rectangles
            .Max(body => body.Position.Z + body.SizeZ / 2);

        var posX = Parts
            .Select(body => body.Position.X)
            .Sum();
        var posY = Parts
            .Select(body => body.Position.Y)
            .Sum();
        var posZ = Parts
            .Select(body => body.Position.Z)
            .Sum();

        return new RectangularCuboid(
            new Vector3(
                (maxX + minX) / 2,
                (maxY + minY) / 2,
                (maxZ + minZ) / 2
            ),
            maxX - minX,
            maxY - minY,
            maxZ - minZ
        );
    }
}