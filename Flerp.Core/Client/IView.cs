using Flerp.DomainModel;
using System;

namespace Flerp.Client
{
    public interface IView
    {
        void BarManager_BarItemClick(object sender, EventArgs e);

        object BarManager_GetBarItems();

        string Name { get; set; }

        void RefreshView(bool saveLayout, Type[] types = null);

        void ShowEditorByEntity(IPersistable entity);
    }
}