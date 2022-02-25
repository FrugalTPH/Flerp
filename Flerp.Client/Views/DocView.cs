using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Helpers;
using DevExpress.XtraGrid.Views.Grid;
using Flerp.Client.Helpers;
using Flerp.Client.Properties;
using Flerp.Commands;
using Flerp.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Flerp.Client.Views
{
    public partial class DocView : XtraForm, IView
    {
        public DocView() { InitializeComponent(); }

        private Binder _binder;
        private Func<IEnumerable<DocumentBase>> _dataSourceDocumentGrid;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e) { }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.DocViewBarItems;
            return barItems;
        }

        private static void DocView_RestoreLayout() { }

        private void DocView_SaveLayout()
        {
            var gView = (GridView)gridControl_document.MainView;
            if (gView != null) gView.XtraGridSaveLayout(Name, Controller.AppSettings);
        }
       
        private void LoadDocumentGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Documents;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<DocumentBase>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<DocumentBase>(x => x.Id, RiProvider.IdEditor(), false, true, 100, 100),
                ColumnDefinition.Create<DocumentBase>(x => x.MasterExtension, RiProvider.TextEditor(), false, false, 40, 40, Resources.Label_Ellipses, Resources.Label_Extension),
                ColumnDefinition.Create<DocumentBase>(x => x.Category, RiProvider.CategoryLookupEditor(), false, false, 100, 100),
                ColumnDefinition.Create<DocumentBase>(x => x.Name, RiProvider.MemoEditor(), false, false, 300, 300),
                ColumnDefinition.Create<DocumentBase>(x => x.Status, RiProvider.LcStatusEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_LifeCycleStatus),
                ColumnDefinition.Create<DocumentBase>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<DocumentBase>(x => x.AliasId, RiProvider.TextEditor(), false, false, 225, 225),
                ColumnDefinition.Create<DocumentBase>(x => x.Source, RiProvider.TextEditor(), false, true, 225, 225),
                ColumnDefinition.Create<DocumentBase>(x => x.Views, RiProvider.IntegerEditor(), false, true, 50, 50),
                ColumnDefinition.Create<DocumentBase>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
            });

            gView.XtragridSetOptions(true);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
            
            gControl.DragOver +=                (sender, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            gControl.DragDrop +=                (sender, e) => Controller.IssueCommand(new GridDropCommand(_binder, (string[])e.Data.GetData(DataFormats.FileDrop)));
            gControl.MouseDown +=               (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove +=               (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.FocusedRowObjectChanged +=    (sender, e) => _mview.RefreshBars();
            //gView.SelectionChanged +=           (sender, e) => _mview.RefreshBars();
            gView.ShowingEditor +=              (sender, e) => gView.Xtragrid_ShowingEditor();           
        }
                      
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DocView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _binder = null;
            _dataSourceDocumentGrid = () => Controller.Collection.OfType<DocumentBase>();
            _mview = (MainView)MdiParent;

            LoadDocumentGrid(gridControl_document);
            DocView_RestoreLayout();
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(DocumentBase), () => gridControl_document.XtraGridRefresh(_dataSourceDocumentGrid.Invoke(), saveLayout) }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = (GridView)gridControl_document.MainView;
            if (gView != null) gView.XtraGridShowEditor(entity, Resources.DbField_Category);
        }
     }
}