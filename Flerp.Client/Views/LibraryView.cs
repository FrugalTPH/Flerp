using DevExpress.XtraBars;
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
    public partial class LibraryView : XtraForm, IView
    {
        public LibraryView() { InitializeComponent(); }

        private Binder _binder;
        private Func<IEnumerable<Document>> _dataSourceDocumentGrid;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;
            if (button == _mview.BiProvider.StubNew && _binder != null) Controller.IssueCommand(new NewStubCommand(_binder));
            if (button == _mview.BiProvider.DocumentNewNull && _binder != null) Controller.IssueCommand(new NewOutputDocumentFromNothingCommand(_binder));

            var document = gridControl_document.XtraGridGetEntity<Document>();
            if (document == null) return;

            if (button == _mview.BiProvider.DocumentCancel) Controller.IssueCommand(new IncrementLifecycleCommand(document));
            else if (button == _mview.BiProvider.DocumentNewCopy) Controller.IssueCommand(new NewOutputDocumentFromDocumentCommand(document));
            else if (button == _mview.BiProvider.DocumentNewRevision) Controller.IssueCommand(new NewOutputDocumentRevisionCommand(document));
            else if (button == _mview.BiProvider.DocumentRelease) Controller.IssueCommand(new IncrementLifecycleCommand(document));
            else if (button == _mview.BiProvider.DocumentConvertToDirectory) document.ToDirectory();
            else if (button == _mview.BiProvider.DocumentConvertToFile) document.ToFile();
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.LibraryViewBarItems;

            (barItems.First(x => x == _mview.BiProvider.StubNew)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.DocumentNewNull)).Enabled = true;

            var docView = (GridView)gridControl_document.MainView;
            var document = (Document)docView.GetFocusedRow();
            if (document != null && docView.SelectedRowsCount == 1)
            {
                (barItems.First(x => x == _mview.BiProvider.DocumentRelease)).Enabled = document.IsReleasable;
                (barItems.First(x => x == _mview.BiProvider.DocumentCancel)).Enabled = document.IsCancellable;
                (barItems.First(x => x == _mview.BiProvider.DocumentNewCopy)).Enabled = document.IsCopyable;
                (barItems.First(x => x == _mview.BiProvider.DocumentNewRevision)).Enabled = document.IsRevisable;
                (barItems.First(x => x == _mview.BiProvider.DocumentConvertToDirectory)).Enabled = document.IsConvertibleToDirectory;
                (barItems.First(x => x == _mview.BiProvider.DocumentConvertToFile)).Enabled = document.IsConvertibleToFile;
            }
            return barItems;
        }

        private void LibraryView_RestoreLayout()
        {
            // Ignore
        }

        private void LibraryView_SaveLayout()
        {
            var grids = new[]
            { 
                (GridView)gridControl_document.MainView,
            };
            foreach (var view in grids) { view.XtraGridSaveLayout(Name, Controller.AppSettings); }
        }

        private void LoadDocumentGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Documents;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Document>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Document>(x => x.Id, RiProvider.IdEditor(), false, true, 100, 100),
                ColumnDefinition.Create<Document>(x => x.MasterExtension, RiProvider.TextEditor(), false, false, 40, 40, Resources.Label_Ellipses, Resources.Label_Extension),
                ColumnDefinition.Create<Document>(x => x.Category, RiProvider.CategoryLookupEditor(), false, false, 100, 100),
                ColumnDefinition.Create<Document>(x => x.Name, RiProvider.MemoEditor(), false, false, 300, 300),
                ColumnDefinition.Create<Document>(x => x.Status, RiProvider.LcStatusEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_LifeCycleStatus),
                ColumnDefinition.Create<Document>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Document>(x => x.AliasId, RiProvider.TextEditor(), false, false, 225, 225),
                ColumnDefinition.Create<Document>(x => x.Source, RiProvider.TextEditor(), false, true, 225, 225),
                ColumnDefinition.Create<Document>(x => x.Views, RiProvider.IntegerEditor(), false, true, 50, 50),
                ColumnDefinition.Create<Document>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Document>(x => x.PrivateNotes, RiProvider.MemoPrivateEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.Label_PrivateNotes)
            });

            gView.XtragridSetOptions(true);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gControl.DragOver += (sender, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            gControl.DragDrop += (sender, e) => Controller.IssueCommand(new GridDropCommand(_binder, (string[])e.Data.GetData(DataFormats.FileDrop)));
            gControl.MouseDown += (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove += (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.FocusedRowObjectChanged += (sender, e) => _mview.RefreshBars();
            //gView.SelectionChanged += (sender, e) => _mview.RefreshBars();
            gView.ShowingEditor += (sender, e) => gView.Xtragrid_ShowingEditor();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            LibraryView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _binder = Controller.GetEntityById<Binder>(Text);
            _dataSourceDocumentGrid = () => Controller.Collection.OfType<Document>().Where(x => x.Id.Contains(_binder.Id));
            _mview = (MainView)MdiParent;

            LoadDocumentGrid(gridControl_document);
            LibraryView_RestoreLayout();
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Document), () => gridControl_document.XtraGridRefresh(_dataSourceDocumentGrid.Invoke(), saveLayout) }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = null;
  
            if (entity is Document) gView = (GridView)gridControl_document.MainView;
            if (gView != null) gView.XtraGridShowEditor(entity, Resources.DbField_Name);
        }
     }
}