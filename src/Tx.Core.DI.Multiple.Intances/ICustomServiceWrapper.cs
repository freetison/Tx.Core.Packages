namespace Tx.Core.DI.Multiple.Instance;

public interface ICustomServiceWrapper<out T>
{
    public string Name { get; }
    public T CustomService { get; }
}