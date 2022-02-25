using Autofac;

namespace Flerp.Data
{
    public interface IController
    {
        void Start(IContainer container);
    }
}