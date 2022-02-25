using Flerp.DomainModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace Flerp.Client
{
    public interface IMainView : ISynchronizeInvoke
    {
        Form Show_FixedView<T>(string name) where T : Form, new();
        
        Form Show_BinderOrPartyView(BinderBase binder);

        void RefreshBars();

        IView ActiveView { get; }
    }
}