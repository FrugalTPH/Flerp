using System.Threading;

namespace Flerp.DomainModel
{
    public interface IBinderable : IPersistable 
    {
        void ToBinder(Binder target, CancellationTokenSource cts);
    }
}