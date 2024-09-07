namespace Incapsulation.RationalNumbers;

public readonly struct Rational
{
    public readonly int Numerator;
    public readonly int Denominator;
    public bool IsNan => Denominator == 0;

    public Rational(int numerator, int denominator = 1)
    {
        var reduced = Reduce(numerator, denominator);
        Numerator = reduced.numerator;
        Denominator = reduced.denominator;
    }

    public static Rational operator +(Rational a, Rational b)
    {
        if (a.IsNan || b.IsNan) return new Rational(1, 0);
        var reduced = Reduce(
            a.Numerator * b.Denominator + b.Numerator * a.Denominator,
            a.Denominator * b.Denominator
        );
        return new Rational(
            reduced.numerator,
            reduced.denominator
        );
    }

    public static Rational operator -(Rational a, Rational b)
    {
        if (a.IsNan || b.IsNan) return new Rational(1, 0);
        var reduced = Reduce(
            a.Numerator * b.Denominator - b.Numerator * a.Denominator,
            a.Denominator * b.Denominator
        );
        return new Rational(
            reduced.numerator,
            reduced.denominator
        );
    }

    public static Rational operator *(Rational a, Rational b)
    {
        if (a.IsNan || b.IsNan) return new Rational(1, 0);
        var reduced = Reduce(
            a.Numerator * b.Numerator,
            a.Denominator * b.Denominator
        );
        return new Rational(
            reduced.numerator,
            reduced.denominator
        );
    }

    public static Rational operator /(Rational a, Rational b)
    {
        if (a.IsNan || b.IsNan) return new Rational(1, 0);
        var reduced = Reduce(
            a.Numerator * b.Denominator,
            a.Denominator * b.Numerator
        );
        return new Rational(
            reduced.numerator,
            reduced.denominator
        );
    }

    public static implicit operator double(Rational a)
    {
        if (a.IsNan) return double.NaN;
        return (double)a.Numerator / a.Denominator;
    }

    public static explicit operator int(Rational a)
    {
        if (a.IsNan) return 0;
        if (a.Denominator != 1) throw new Exception();
        return a.Numerator / a.Denominator;
    }

    public static implicit operator Rational(int value)
    {
        return new Rational(value);
    }

    private static (int numerator, int denominator) Reduce(int numerator, int denominator)
    {
        if (denominator == 0) return (numerator, denominator);
        if (numerator == 0) return (0, 1);
        var start = Math.Max(
            Math.Abs(numerator),
            Math.Abs(denominator)
        );
        var divider = Math.Min(
            Math.Abs(numerator),
            Math.Abs(denominator)
        );
        while (start % divider != 0)
        {
            var result = start % divider;
            start = divider;
            divider = result;
        }

        return denominator < 0
            ? (-numerator / divider, -denominator / divider)
            : (numerator / divider, denominator / divider);
    }
}