namespace Tx.Core.DI.Multiple.Intances;

public interface ICustomServiceWrapper<out T>
{
    public string Name { get; }
    public T CustomService { get; }
}