namespace Tx.Core.DI.Multiple.Instance;

public interface ICustomServiceResolver<out T>
{
    T GetServiceByName(string name);
}