using System.Drawing;

namespace Inheritance.Geometry.Visitor;

public interface IVisitor
{
    Body Visit(Ball ball);
    Body Visit(RectangularCuboid cuboid);
    Body Visit(Cylinder cylinder);
    Body Visit(CompoundBody compound);
}

public abstract class Body
{
    public Vector3 Position { get; }
    public abstract Body Accept(IVisitor visitor);

    protected Body(Vector3 position)
    {
        Position = position;
    }
}

public class Ball : Body
{
    public double Radius { get; }

    public Ball(Vector3 position, double radius) : base(position)
    {
        Radius = radius;
    }

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
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

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
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

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class CompoundBody : Body
{
    public IReadOnlyList<Body> Parts { get; }

    public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
    {
        Parts = parts;
    }

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class BoundingBoxVisitor : IVisitor
{
    public Body Visit(Ball ball)
    {
        return new RectangularCuboid(
            ball.Position,
            2 * ball.Radius,
            2 * ball.Radius,
            2 * ball.Radius
        );
    }

    public Body Visit(RectangularCuboid cuboid)
    {
        return new RectangularCuboid(
            cuboid.Position,
            cuboid.SizeX,
            cuboid.SizeY,
            cuboid.SizeZ
        );
    }

    public Body Visit(Cylinder cylinder)
    {
        return new RectangularCuboid(
            cylinder.Position,
            2 * cylinder.Radius,
            2 * cylinder.Radius,
            cylinder.SizeZ
        );
    }

    public Body Visit(CompoundBody compound)
    {
        var rectangles = compound.Parts
            .Select(part => part.Accept(this) as RectangularCuboid)
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

public class BoxifyVisitor : IVisitor
{
    public Body Visit(Ball ball)
    {
        return new RectangularCuboid(
            ball.Position,
            2 * ball.Radius,
            2 * ball.Radius,
            2 * ball.Radius
        );
    }

    public Body Visit(RectangularCuboid cuboid)
    {
        return new RectangularCuboid(
            cuboid.Position,
            cuboid.SizeX,
            cuboid.SizeY,
            cuboid.SizeZ
        );
    }

    public Body Visit(Cylinder cylinder)
    {
        return new RectangularCuboid(
            cylinder.Position,
            2 * cylinder.Radius,
            2 * cylinder.Radius,
            cylinder.SizeZ
        );
    }

    public Body Visit(CompoundBody compound)
    {
        return new CompoundBody(compound.Parts
            .Select(part => part.Accept(this))
            .ToList());
    }
}