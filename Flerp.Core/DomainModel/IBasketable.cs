using System.Threading;

namespace Flerp.DomainModel
{
    public interface IBasketable : IPersistable
    {
        void ToBasket(CancellationTokenSource cts);
    }
}