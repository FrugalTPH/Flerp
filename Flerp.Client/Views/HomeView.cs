using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Helpers;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class HomeView : XtraForm, IView
    {
        public HomeView() { InitializeComponent(); }

        private Func<IEnumerable<Binder>> _dataSourceBinderGrid;
        private Func<IEnumerable<Email>> _dataSourceEmailGrid;
        private Func<IEnumerable<Party>> _dataSourcePartyGrid;
        private Func<IEnumerable<Stub>> _dataSourceStubGrid;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var entity = gridControl_party.XtraGridGetEntity<IPersistable>();

            Party party = null;
            if (entity is PartyDetail) party = Controller.GetEntityById<Party>(((PartyDetail)entity).MajorId);
            else if (entity is Party) party = (Party)entity;

            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;

            if (button == _mview.BiProvider.PartyNewHuman) Controller.IssueCommand(new NewHumanCommand());
            else if (button == _mview.BiProvider.PartyNewOrganisation) Controller.IssueCommand(new NewOrganisationCommand());
            else if (button == _mview.BiProvider.BinderNewAdmin) Controller.IssueCommand(new NewAdminBinderCommand());
            else if (button == _mview.BiProvider.BinderNewWork) Controller.IssueCommand(new NewWorkBinderCommand());
            else if (button == _mview.BiProvider.PartyNewPartyDetail && party != null) Controller.IssueCommand(new NewPartyDetailCommand(party));
            else if (button == _mview.BiProvider.StubView) _mview.Show_StubView();
            else if (button == _mview.BiProvider.DocView) _mview.Show_DocView();
            else if (button == _mview.BiProvider.EmailGet) Controller.IssueCommand(new CrawlEmailCommand());
            else if (button == _mview.BiProvider.AppSettings) _mview.Show_AppSettingsView();
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.HomeViewBarItems;

            var partyView = (GridView)gridControl_party.MainView;
            (barItems.First(x => x == _mview.BiProvider.BinderNewAdmin)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.BinderNewWork)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.PartyNewHuman)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.PartyNewOrganisation)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.PartyNewPartyDetail)).Enabled = partyView.SelectedRowsCount == 1;
            (barItems.First(x => x == _mview.BiProvider.StubView)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.DocView)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.EmailGet)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.AppSettings)).Enabled = true;
            return barItems;
        }

        private void HomeView_RestoreLayout()
        {
            var layout = Controller.AppSettings.GetById(Resources.Layout_Prefix + Name);
            if (layout == null) return;

            var obj = JsonConvert.DeserializeAnonymousType(layout.Value, new { arg1 = 0, arg2 = 0, arg3 = 0 }).ThrowIfNull();
            splitContainerControl1.SplitterPosition = obj.arg1;
            splitContainerControl2.SplitterPosition = obj.arg2;
            splitContainerControl3.SplitterPosition = obj.arg3;
            RefreshView(false);
        }

        private void HomeView_SaveLayout()
        {
            var grids = new[] 
            { 
                (GridView)gridControl_binder.MainView,
                (GridView)gridControl_party.MainView,
                (GridView)gridControl_email.MainView,
                (GridView)gridControl_stub.MainView,
                (GridView)gridControl_party.LevelTree.Nodes[Resources.Term_Details].LevelTemplate,
            };
            foreach (var view in grids) view.XtraGridSaveLayout(Name, Controller.AppSettings);

            var json = JsonConvert.SerializeObject(
                new
                {
                    arg1 = splitContainerControl1.SplitterPosition,
                    arg2 = splitContainerControl2.SplitterPosition,
                    arg3 = splitContainerControl3.SplitterPosition
                }).ThrowIfNull();

            Controller.AppSettings.Save(new AppSetting(Resources.Layout_Prefix + Name, json));
        }

        private void LoadBinderGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_AllBinders;
            
            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Binder>(x => x.Privacy, RiProvider.PrivacyEditor() , false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Binder>(x => x.Id, RiProvider.IdEditor(), false, true, 50, 50),
                ColumnDefinition.Create<Binder>(x => x.Name, RiProvider.MemoEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Binder>(x => x.Client, RiProvider.PartyLookupEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Binder>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Binder>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Binder>(x => x.Description, RiProvider.MemoExEditor(), true, false, 100, 100),
                ColumnDefinition.Create<Binder>(x => x.HourRate, RiProvider.CurrencyEditor(), true, false, 60, 60),
                ColumnDefinition.Create<Binder>(x => x.MileRate, RiProvider.CurrencyEditor(), true, false, 60, 60),
                ColumnDefinition.Create<Binder>(x => x.ExpenseRate, RiProvider.CurrencyEditor(), true, false, 60, 60),
                ColumnDefinition.Create<Binder>(x => x.DisposedDate, RiProvider.DateEditor(), true, true, 115, 115)
            });
            
            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }
        
        private void LoadEmailGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_EmailsPendingRelease;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Email>(x => x.IsIgnorable, RiProvider.IsIgnorableEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.Label_IsIgnorable),
                ColumnDefinition.Create<Email>(x => x.Id, RiProvider.IdEditor(), false, true, 100, 100),
                ColumnDefinition.Create<Email>(x => x.Name, RiProvider.MemoEditor(), false, true, 225, 225,Resources.Label_Subject),
                ColumnDefinition.Create<Email>(x => x.Source, RiProvider.MemoExEditor(), false, true, 200, 200, Resources.Label_From),
                ColumnDefinition.Create<Email>(x => x.To, RiProvider.MemoExEditor(), false, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Attachments, RiProvider.IntegerEditor(), false, true, 40, 40, Resources.Label_Ellipses, Resources.Label_Attachments),
                ColumnDefinition.Create<Email>(x => x.CreatedDate, RiProvider.DateEditor(), false, true, 115, 115, Resources.Label_Received),
                ColumnDefinition.Create<Email>(x => x.ModifiedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Email>(x => x.Cc, RiProvider.MemoExEditor(), true, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Bcc, RiProvider.MemoExEditor(), true, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Views, RiProvider.IntegerEditor(), true, true, 40, 40),
                ColumnDefinition.Create<Email>(x => x.DisposedDate, RiProvider.DateEditor(), true, true, 115, 115, Resources.Label_Deleted),
                ColumnDefinition.Create<Email>(x => x.Category, RiProvider.CategoryLookupEditor(), true, true, 100, 100),
                ColumnDefinition.Create<Email>(x => x.MasterExtension, RiProvider.TextEditor(), true, true, 110, 100),
                ColumnDefinition.Create<Email>(x => x.MasterHash, RiProvider.TextEditor(), true, true, 100, 100),
                ColumnDefinition.Create<Email>(x => x.Status, RiProvider.LcStatusEditor(), true, true, 27, 27),
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gControl.DragOver += (sender, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            gControl.DragDrop += (sender, e) => Controller.IssueCommand(new EmailDropCommand((string[])e.Data.GetData(DataFormats.FileDrop)));
            gControl.MouseDown += (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove += (sender, e) => gControl.Xtragrid_MouseMove(e);
        }

        private void LoadPartyGrid(GridControl gControl)
        {
            var gView =(GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_AllParties;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Party>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Party>(x => x.Id, RiProvider.IdEditor(), false, true, 50, 50),
                ColumnDefinition.Create<Party>(x => x.Name, RiProvider.TextEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Party>(x => x.Description, RiProvider.TextEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Party>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Party>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Party>(x => x.DisposedDate, RiProvider.DateEditor(), true, true, 115, 115, Resources.Label_Deleted),
            });

            gView.XtragridSetOptions(true);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gControl.MouseDown += (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove += (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.FocusedRowObjectChanged += (sender, e) => _mview.RefreshBars();
            //gView.SelectionChanged += (sender, e) => _mview.RefreshBars();
        }
        
        private void LoadPartyDetailGrid(GridControl gControl)
        {
            var pView = new GridView(gControl) { Name = Resources.homeView_partyDetailsView, ViewCaption = Resources.Term_Details };

            pView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyDetail>(x => x.DetailType, RiProvider.DetailTypeEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.Label_Type),
                ColumnDefinition.Create<PartyDetail>(x => x.Name, RiProvider.TextEditor(), false, false, 125, 125, Resources.DbField_Description),
                ColumnDefinition.Create<PartyDetail>(x => x.MinorId, RiProvider.MemoEditor(), false, false, 325, 325, Resources.Label_Value),
                ColumnDefinition.Create<PartyDetail>(x => x.Id, RiProvider.DeleteButtonEditor(), false, true, 32, 32, Resources.Label_Ellipses, Resources.Label_Delete),
            });

            gControl.LevelTree.Nodes.Add(pView.ViewCaption, pView);

            pView.XtragridSetOptions(false);
            pView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            pView.ValidatingEditor += (sender, e) => pView.Xtragrid_ValidatingEditor((IPersistable)pView.GetFocusedRow(), e);
        }
        
        private void LoadStubGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_ActionableStubs;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Stub>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Stub>(x => x.Id, RiProvider.IdEditor(), false, true, 80, 80),
                ColumnDefinition.Create<Stub>(x => x.Category, RiProvider.CategoryLookupEditor(), false, false, 100, 100),
                ColumnDefinition.Create<Stub>(x => x.Name, RiProvider.MemoEditor(), false, false, 200, 200),
                ColumnDefinition.Create<Stub>(x => x.IsComplete, RiProvider.IsCompleteEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_TaskComplete),
                ColumnDefinition.Create<Stub>(x => x.ActionableDate, RiProvider.DateEditor(), false, false, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.ClosingRemark, RiProvider.MemoEditor(), false, false, 200, 200),
                ColumnDefinition.Create<Stub>(x => x.ClosingDate, RiProvider.DateEditor(), true, false, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Stub>(x => x.DisposedDate, RiProvider.DateEditor(), true, true, 115, 115, Resources.Label_Deleted)
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
            HomeView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _dataSourceBinderGrid = () => Controller.Collection.OfType<Binder>();
            _dataSourceEmailGrid = () => Controller.Collection.OfType<Email>().Where(x => Equals(x.Status, LcStatusType.Pending) && !x.IsIgnorable);
            _dataSourcePartyGrid = () => Controller.Collection.OfType<Party>();
            _dataSourceStubGrid = () => Controller.Collection.OfType<Stub>().Where(x => x.IsComplete.HasValue && !x.IsComplete.Value);
            _mview = (MainView)MdiParent;

            LoadPartyGrid(gridControl_party);
            LoadPartyDetailGrid(gridControl_party);
            LoadBinderGrid(gridControl_binder);
            LoadEmailGrid(gridControl_email);
            LoadStubGrid(gridControl_stub);
            HomeView_RestoreLayout();            
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Binder), () => gridControl_binder.XtraGridRefresh(_dataSourceBinderGrid.Invoke(), saveLayout) },
                { typeof(Email), () => gridControl_email.XtraGridRefresh(_dataSourceEmailGrid.Invoke(), saveLayout) },
                { typeof(Stub), () => gridControl_stub.XtraGridRefresh(_dataSourceStubGrid.Invoke(), saveLayout) },
                { typeof(Party), () => { gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout); RiProvider.RefreshPartyLookupEditor((GridView)gridControl_binder.MainView); }  },
                { typeof(PartyDetail), () => { gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout); }  }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = null;
            var col = Resources.DbField_Name;

            if (entity is Binder) gView = (GridView)gridControl_binder.MainView;
            else if (entity is Party) gView = (GridView)gridControl_party.MainView;
            else if (entity is PartyDetail)
            {
                var party = Controller.GetEntityById<Party>(((PartyDetail)entity).MajorId);
                var mView = (GridView)gridControl_party.MainView;
                var row = mView.LocateByValue(Resources.DbField_Id, party.Id);
                gridControl_party.PerformSafely(() => mView.ExpandMasterRow(row));
                gView = (GridView)mView.GetDetailView(row, 0);
                col = null;
            }

            if (gView != null) gView.XtraGridShowEditor(entity, col);
        }
    }
}