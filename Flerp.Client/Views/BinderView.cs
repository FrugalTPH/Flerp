using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Helpers;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Helpers;
using Flerp.Client.Helpers;
using Flerp.Client.Properties;
using Flerp.Commands;
using Flerp.DomainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Flerp.Client.Views
{
    public partial class BinderView : XtraForm, IView
    {
        public BinderView() { InitializeComponent(); }

        private Binder _binder;
        private Func<IEnumerable<Document>> _dataSourceDocumentGrid;
        private Func<IEnumerable<PartyBinder>> _dataSourcePartyGrid;
        private Func<IEnumerable<Stub>> _dataSourceStubGrid;
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
            var barItems = _mview.BiProvider.BinderViewBarItems;

            (barItems.First(x => x == _mview.BiProvider.StubNew)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.DocumentNewNull)).Enabled = true;

            var docView = (GridView)gridControl_document.MainView;
            var document = (Document)docView.GetFocusedRow();
            if (document == null || docView.SelectedRowsCount != 1) return barItems;

            (barItems.First(x => x == _mview.BiProvider.DocumentRelease)).Enabled = document.IsReleasable;
            (barItems.First(x => x == _mview.BiProvider.DocumentCancel)).Enabled = document.IsCancellable;
            (barItems.First(x => x == _mview.BiProvider.DocumentNewCopy)).Enabled = document.IsCopyable;
            (barItems.First(x => x == _mview.BiProvider.DocumentNewRevision)).Enabled = document.IsRevisable;
            (barItems.First(x => x == _mview.BiProvider.DocumentConvertToDirectory)).Enabled = document.IsConvertibleToDirectory;
            (barItems.First(x => x == _mview.BiProvider.DocumentConvertToFile)).Enabled = document.IsConvertibleToFile;
            return barItems;
        }

        private void _binder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            propertyGridControl_binder.PerformSafely(Refresh);
        }
        
        private void BinderView_RestoreLayout()
        {
            var layout = Controller.AppSettings.GetById(Resources.Layout_Prefix + Name);
            if (layout == null) return;

            var obj = JsonConvert.DeserializeAnonymousType(layout.Value, new { arg1 = 99, arg2 = 99 }).ThrowIfNull();

            splitContainerControl1.SplitterPosition = obj.arg1;
            splitContainerControl2.SplitterPosition = obj.arg2;
        }

        private void BinderView_SaveLayout()
        {
            var grids = new[] 
            { 
                (GridView)gridControl_party.MainView, 
                (GridView)gridControl_stub.MainView, 
                (GridView)gridControl_document.MainView,
                (GridView)gridControl_party.LevelTree.Nodes[Resources.Term_Details].LevelTemplate,
            };
            foreach (var view in grids) { view.XtraGridSaveLayout(Name, Controller.AppSettings); }

            var json = JsonConvert.SerializeObject(
                new
                {
                    arg1 = splitContainerControl1.SplitterPosition,
                    arg2 = splitContainerControl2.SplitterPosition
                }).ThrowIfNull();

            Controller.AppSettings.Save(new AppSetting(Resources.Layout_Prefix + Name, json));
        }

        private void LoadBinderGrid(PropertyGridControl gControl)
        {
            gControl.XtraVerticalGridInitializeRows(new[]
            {
                ColumnDefinition.Create<Binder>(x => x.Client, RiProvider.PartyLookupEditor(), false, false),
                ColumnDefinition.Create<Binder>(x => x.Name, RiProvider.MemoEditor(), false, false),
                ColumnDefinition.Create<Binder>(x => x.Privacy, RiProvider.PrivacyEditorPg(), false, false),
                ColumnDefinition.Create<Binder>(x => x.Description, RiProvider.MemoEditor(), false, false),
                ColumnDefinition.Create<Binder>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true),
                ColumnDefinition.Create<Binder>(x => x.CreatedDate, RiProvider.DateEditor(), false, true),
                ColumnDefinition.Create<Binder>(x => x.HourRate, RiProvider.CurrencyEditor(), false, false),
                ColumnDefinition.Create<Binder>(x => x.MileRate, RiProvider.CurrencyEditor(), false, false),
                ColumnDefinition.Create<Binder>(x => x.ExpenseRate, RiProvider.CurrencyEditor(), false, false),
            });

            gControl.XtraVerticalGridSetOptions();
            gControl.XtraVerticalGridPopulate(_binder);

            _binder.PropertyChanged += _binder_PropertyChanged;
            gControl.CellValueChanged +=    (sender, e) => _binder.Save();
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
            
            gControl.DragOver +=                (sender, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            gControl.DragDrop +=                (sender, e) => Controller.IssueCommand(new GridDropCommand(_binder, (string[])e.Data.GetData(DataFormats.FileDrop)));
            gControl.MouseDown +=               (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove +=               (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.FocusedRowObjectChanged +=    (sender, e) => _mview.RefreshBars();
            //gView.SelectionChanged +=           (sender, e) => _mview.RefreshBars();
            gView.ShowingEditor +=              (sender, e) => gView.Xtragrid_ShowingEditor();           
        }
      
        private void LoadPartyGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Parties1;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyBinder>(x => x.MajorId, RiProvider.IdEditor(), false, true, 50, 50, Resources.DbField_Id),
                ColumnDefinition.Create<PartyBinder>(x => x.PartyName, RiProvider.TextEditor(), false, true, 175, 175, Resources.DbField_Name),
                ColumnDefinition.Create<PartyBinder>(x => x.Name, RiProvider.TextEditor(), false, false, 150, 150, Resources.Label_Role),
                ColumnDefinition.Create<PartyBinder>(x => x.Id, RiProvider.DeleteButtonEditor(), false, true, 32, 32, Resources.Label_Ellipses, Resources.Label_Delete),
            });

            gView.XtragridSetOptions(true);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }
        
        private void LoadPartyDetailGrid(GridControl gControl)
        {
            var pView = new GridView(gControl) { Name = Resources.binderView_partyDetailsView, ViewCaption = Resources.Term_Details };

            pView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyDetail>(x => x.DetailType, RiProvider.DetailTypeEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_Type),
                ColumnDefinition.Create<PartyDetail>(x => x.Name, RiProvider.TextEditor(), false, true, 100, 100, Resources.DbField_Description),
                ColumnDefinition.Create<PartyDetail>(x => x.MinorId, RiProvider.MemoEditor(), false, true, 225, 225, Resources.Label_Value),
            });

            gControl.LevelTree.Nodes.Add(pView.ViewCaption, pView);

            pView.XtragridSetOptions(false);
            pView.XtraGridRestoreLayout(Name, Controller.AppSettings);
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
            BinderView_SaveLayout();
            _binder.PropertyChanged -= _binder_PropertyChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _binder = Controller.GetEntityById<Binder>(Text);
            _dataSourceDocumentGrid = () => Controller.Collection.OfType<Document>().Where(x => x.Id.Contains(_binder.Id));
            _dataSourcePartyGrid = () => Controller.Collection.OfType<PartyBinder>().Where(x => x.MinorId == _binder.Id);
            _dataSourceStubGrid = () => Controller.Collection.OfType<Stub>().Where(x => x.Id.Contains(_binder.Id));
            _mview = (MainView)MdiParent;

            LoadBinderGrid(propertyGridControl_binder);
            LoadPartyGrid(gridControl_party);
            LoadStubGrid(gridControl_stub);
            LoadDocumentGrid(gridControl_document);
            LoadPartyDetailGrid(gridControl_party);
            BinderView_RestoreLayout();
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Document), () => gridControl_document.XtraGridRefresh(_dataSourceDocumentGrid.Invoke(), saveLayout) },
                { typeof(Stub), () => gridControl_stub.XtraGridRefresh(_dataSourceStubGrid.Invoke(), saveLayout) },
                { typeof(Party), () => { gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout); RiProvider.RefreshPartyLookupEditor(propertyGridControl_binder.ThrowIfNull()); }  },
                { typeof(PartyBinder), () => { gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout); RiProvider.RefreshPartyLookupEditor(propertyGridControl_binder.ThrowIfNull()); }  },
                { typeof(PartyDetail), () => { gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout); RiProvider.RefreshPartyLookupEditor(propertyGridControl_binder.ThrowIfNull()); }  }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = null;

            if (entity is Stub) gView = (GridView)gridControl_stub.MainView;
            else if (entity is Document) gView = (GridView)gridControl_document.MainView;

            if (gView != null) gView.XtraGridShowEditor(entity, Resources.DbField_Category);
        }
     }
}