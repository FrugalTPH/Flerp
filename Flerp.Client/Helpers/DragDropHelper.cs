using DevExpress.Utils.Mdi;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraTabbedMdi;
using Flerp.Client.Views;
using Flerp.Commands;
using Flerp.DomainModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Flerp.Client.Helpers
{
    public class DragDropHelper
    {
        private readonly XtraTabbedMdiManager _manager;
        private MdiClient MdiClient { get { return MdiClientSubclasser.GetMdiClient(_manager.MdiParent); } }

        private string Target { get; set; }

        private bool AllowHeaderDragOver
        {
            set
            {
                MdiClient.DragOver -= OnMdiClientDragOver;
                if (value) { MdiClient.DragOver += OnMdiClientDragOver; }
            }
        }

        private bool AllowHeaderDrop
        {
            set
            {
                MdiClient.DragDrop -= OnMdiClientDragDrop;
                if (value) { MdiClient.DragDrop += OnMdiClientDragDrop; }
            }
        }

        public DragDropHelper(XtraTabbedMdiManager manager)
        {
            _manager = manager;

            AllowHeaderDragOver = true;
            AllowHeaderDrop = true;
        }


        private void AcceptDrop(string target, DragEventArgs e)
        {
            Target = target;
            e.Effect = DragDropEffects.Copy;
        }

        private void OnMdiClientDragOver(object sender, DragEventArgs e)
        {
            try
            {
                var p = (_manager as IXtraTab).ScreenPointToControl(new Point(e.X, e.Y));
                e.Effect = DragDropEffects.None;

                var hitInfo = _manager.CalcHitInfo(p);
                if (hitInfo.HitTest != XtraTabHitTest.PageHeader) return;

                var xtraMdiTabPage = hitInfo.Page as XtraMdiTabPage;

                var targetForm = xtraMdiTabPage != null && xtraMdiTabPage.MdiChild != _manager.SelectedPage.MdiChild 
                    ? xtraMdiTabPage.MdiChild 
                    : null;

                var ent = ((string[]) e.Data.GetData(typeof (string[]))).First();
                var dropdataSpecimen = Controller.GetEntityById<IPersistable>(ent);
                
                if ((targetForm is BinderView && dropdataSpecimen is IBinderable) ||
                    (targetForm is BasketView && dropdataSpecimen is IBasketable) ||
                    (targetForm is LibraryView && dropdataSpecimen is ILibraryable) ||
                    (targetForm is PartyView && dropdataSpecimen is IPartyable)) 
                    
                    AcceptDrop(targetForm.Text, e);
            }
            catch
            {
                // ignored
            }
        }

        private void OnMdiClientDragDrop(object sender, DragEventArgs e)
        {
            var str = (string[])e.Data.GetData(typeof(string[]));
            if (str != null) Controller.IssueCommand(new TabDropCommand(Target, str));         
        }
    
    }
}
