namespace Inheritance.DataStructure;

public class Category : IComparable
{
    public string Name { get; }
    public MessageType MessageType { get; }
    public MessageTopic MessageTopic { get; }

    public Category(string name, MessageType type, MessageTopic topic)
    {
        Name = name;
        MessageType = type;
        MessageTopic = topic;
    }

    public override int GetHashCode()
    {
        return (Name, MessageType, MessageTopic).GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Category category)
            return 1;
        return (Name, MessageType, MessageTopic)
            .CompareTo(
                (category.Name, category.MessageType, category.MessageTopic)
            );
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Category category)
            return false;
        return CompareTo(category) == 0;
    }

    public static bool operator >(Category category1, Category category2)
    {
        return category1.CompareTo(category2) > 0;
    }

    public static bool operator <(Category category1, Category category2)
    {
        return category1.CompareTo(category2) < 0;
    }

    public static bool operator >=(Category category1, Category category2)
    {
        return category1.CompareTo(category2) >= 0;
    }

    public static bool operator <=(Category category1, Category category2)
    {
        return category1.CompareTo(category2) <= 0;
    }

    public override string ToString()
    {
        return $"{Name}.{MessageType}.{MessageTopic}";
    }
}