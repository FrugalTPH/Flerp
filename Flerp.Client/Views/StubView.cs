using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Helpers;
using DevExpress.XtraGrid.Views.Grid;
using Flerp.Client.Helpers;
using Flerp.Client.Properties;
using Flerp.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Flerp.Client.Views
{
    public partial class StubView : XtraForm, IView
    {
        public StubView() { InitializeComponent(); }

        private Func<IEnumerable<Stub>> _dataSourceStubGrid;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e) { }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.StubViewBarItems;
            return barItems;
        }
        
        private static void StubView_RestoreLayout() { }

        private void StubView_SaveLayout()
        {
            var gView = (GridView)gridControl_stub.MainView;
            if (gView != null) gView.XtraGridSaveLayout(Name, Controller.AppSettings);
        }
                
        private void LoadStubGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Stubs;
            
            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Stub>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Stub>(x => x.Id, RiProvider.IdEditor(), false, true, 80, 80),
                ColumnDefinition.Create<Stub>(x => x.Category, RiProvider.CategoryLookupEditor(), false, false, 100, 100),
                ColumnDefinition.Create<Stub>(x => x.CreatedDate, RiProvider.DateEditor(), false, false, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.Name, RiProvider.MemoEditor(), false, false, 250, 250),
                ColumnDefinition.Create<Stub>(x => x.IsComplete, RiProvider.IsCompleteEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_TaskComplete),
                ColumnDefinition.Create<Stub>(x => x.ActionableDate, RiProvider.DateEditor(), false, false, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.ClosingDate, RiProvider.DateEditor(), false, false, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.ClosingRemark, RiProvider.MemoEditor(), false, false, 250, 250),
                ColumnDefinition.Create<Stub>(x => x.ModifiedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.Reference, RiProvider.TextEditor(), false, false, 100, 100)
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gControl.MouseDown += (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove += (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.ShowingEditor += (sender, e) => gView.Xtragrid_ShowingEditor();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StubView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _dataSourceStubGrid = () => Controller.Collection.OfType<Stub>();
            _mview = (MainView)MdiParent;

            LoadStubGrid(gridControl_stub);
            StubView_RestoreLayout();
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Stub), () => gridControl_stub.XtraGridRefresh(_dataSourceStubGrid.Invoke(), saveLayout) }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = (GridView)gridControl_stub.MainView;
            if (gView != null) gView.XtraGridShowEditor(entity, Resources.DbField_Category);
        }
     }
}