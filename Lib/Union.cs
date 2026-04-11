namespace System.Runtime.CompilerServices;

public interface IUnion
{
    public object Value { get; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
sealed class UnionAttribute() : Attribute;
