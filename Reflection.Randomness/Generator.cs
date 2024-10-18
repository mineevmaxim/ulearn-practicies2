using System.Reflection;

namespace Reflection.Randomness;

public class FromDistributionAttribute : Attribute
{
    public readonly Type DistributionType;
    public readonly int[] Values;
    public FromDistributionAttribute(Type type, params int[] values)
    {
        if (!type.IsAssignableTo(typeof(IContinuousDistribution)))
            throw new ArgumentException($"Type '{type.FullName}' is not derived from IContinuousDistribution.");
        if (type.GetConstructors().All(c => c.GetParameters().Length != values.Length))
            throw new ArgumentException($"Type '{type.FullName}' wrong count of arguments.");
        DistributionType = type;
        Values = values;
    }
}

public class Generator<T>
    where T : new()
{
    private static readonly PropertyInfo[] properties;
    private static readonly List<IContinuousDistribution>? distributions;

    static Generator()
    {
        properties = typeof(T)
            .GetProperties()
            .Where(p => p.GetCustomAttributes<FromDistributionAttribute>().Any())
            .ToArray();
        var attributes = properties
            .Select(p => p.GetCustomAttributes<FromDistributionAttribute>().First())
            .ToArray();

        for (var i = 0; i < properties.Length; i++)
        {
            var attribute = attributes[i];
            var types = attribute.Values.Select(value => value.GetType()).ToArray();
            var constructor = attribute.DistributionType.GetConstructor(types);
            if (constructor is null) // 
                throw new ArgumentException($"Type '{typeof(T).FullName}' wrong count of arguments.");
            distributions ??= new List<IContinuousDistribution>();
            distributions.Add((IContinuousDistribution)constructor
                    .Invoke(attribute.Values.Select(v => (object)v).ToArray()));
        }
    }

    public T Generate(Random rnd)
    {
        var result = new T();
        for (var i = 0; i < properties.Length; i++)
            properties[i].SetValue(result, distributions?[i].Generate(rnd));
        return result;
    }
}