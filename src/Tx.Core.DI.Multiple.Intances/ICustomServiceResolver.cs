namespace Tx.Core.DI.Multiple.Intances;

public interface ICustomServiceResolver<out T>
{
    T GetServiceByName(string name);
}