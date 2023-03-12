namespace Quantum.Lib.Common;

public interface ISpecifiable
{
    object Value { get; }

    bool IsSpecified { get; }
}
