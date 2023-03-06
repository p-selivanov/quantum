namespace Quantum.Lib.Common;

/// <summary>
/// Represents a value that might be unspecified (similar to Nullable). 
/// Intended to be used in Json serialization when some field might not be presend in Json.
/// </summary>
public struct Specifiable<T> : ISpecifiable
{
    public T Value { get; }

    public bool IsSpecified { get; }

    object ISpecifiable.Value => Value;

    public Specifiable(T value, bool isSpecified = true)
    {
        Value = value;
        IsSpecified = isSpecified;
    }

    public override bool Equals(object other)
    {
        if (other is Specifiable<T> otherSpecifiable)
        {
            return
                IsSpecified == otherSpecifiable.IsSpecified &&
                object.Equals(Value, otherSpecifiable.Value);
        }

        return false;
    }

    public override int GetHashCode()
    {
        if (!IsSpecified ||
            Value is null)
        {
            return 0;
        }

        return Value.GetHashCode();
    }

    public override string ToString()
    {
        if (!IsSpecified)
        {
            return $"{base.ToString()}(unspecified)";
        }

        if (Value is null)
        {
            return $"{base.ToString()}(null)";
        }

        return $"{base.ToString()}({Value})";
    }

    public static implicit operator Specifiable<T>(T value)
    {
        return new Specifiable<T>(value);
    }

    public static bool operator ==(Specifiable<T> left, Specifiable<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Specifiable<T> left, Specifiable<T> right)
    {
        return !(left == right);
    }

    public static Specifiable<T> Unspecified()
    {
        return new Specifiable<T>();
    }
}
