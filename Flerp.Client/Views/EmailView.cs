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
    public partial class EmailView : XtraForm, IView
    {
        public EmailView() { InitializeComponent(); }

        private Binder _binder;
        private Func<IEnumerable<Email>> _dataSourceEmailGrid;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var email = gridControl_email.XtraGridGetEntity<Email>();

            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;
            if (button == _mview.BiProvider.StubNew && _binder != null) Controller.IssueCommand(new NewStubCommand(_binder));
            else if (button == _mview.BiProvider.EmailGet) Controller.IssueCommand(new CrawlEmailCommand());
            else if (button == _mview.BiProvider.EmailCancel && email != null) Controller.IssueCommand(new IncrementLifecycleCommand(email));
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.EmailViewBarItems;

            (barItems.First(x => x == _mview.BiProvider.StubNew)).Enabled = true;
            (barItems.First(x => x == _mview.BiProvider.EmailGet)).Enabled = true;

            var emailView = (GridView)gridControl_email.MainView;
            var email = (Email)emailView.GetFocusedRow();
            if (email == null || emailView.SelectedRowsCount != 1) return barItems;

            (barItems.First(x => x == _mview.BiProvider.EmailCancel)).Enabled = email.IsCancellable;
            return barItems;
        }

        private void EmailView_SaveLayout()
        {
            var grids = new[] { (GridView)gridControl_email.MainView };
            foreach (var view in grids) { view.XtraGridSaveLayout(Name, Controller.AppSettings); }
        }

        private void LoadEmailGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Emails;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Email>(x => x.Privacy, RiProvider.PrivacyEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Email>(x => x.IsIgnorable, RiProvider.IsIgnorableEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.Label_IsIgnorable),
                ColumnDefinition.Create<Email>(x => x.Id, RiProvider.IdEditor(), false, true, 100, 100),
                ColumnDefinition.Create<Email>(x => x.Name, RiProvider.MemoEditor(), false, true, 300, 300, Resources.Label_Subject),
                ColumnDefinition.Create<Email>(x => x.Status, RiProvider.LcStatusEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_LifeCycleStatus),
                ColumnDefinition.Create<Email>(x => x.Source, RiProvider.MemoExEditor(), false, true, 225, 225, Resources.Label_From),
                ColumnDefinition.Create<Email>(x => x.To, RiProvider.MemoExEditor(), false, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Attachments, RiProvider.IntegerEditor(), false, true, 40, 40, Resources.Label_Ellipses, Resources.Label_Attachments),
                ColumnDefinition.Create<Email>(x => x.CreatedDate, RiProvider.DateEditor(), false, false, 115, 115, Resources.Label_Received),
                ColumnDefinition.Create<Email>(x => x.ModifiedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Email>(x => x.Cc, RiProvider.MemoExEditor(), true, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Bcc, RiProvider.MemoExEditor(), true, true, 200, 200),
                ColumnDefinition.Create<Email>(x => x.Views, RiProvider.IntegerEditor(), false, true, 50, 50),
                ColumnDefinition.Create<Email>(x => x.Category, RiProvider.CategoryLookupEditor(), true, true, 100, 100),
                ColumnDefinition.Create<Email>(x => x.MasterExtension, RiProvider.TextEditor(), true, true, 40, 40, Resources.Label_Ellipses, Resources.Label_Extension),
            });

            gView.XtragridSetOptions(true);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gControl.DragOver += (sender, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            gControl.DragDrop += (sender, e) => Controller.IssueCommand(new EmailDropCommand((string[])e.Data.GetData(DataFormats.FileDrop)));
            gControl.MouseDown += (sender, e) => gView.Xtragrid_MouseDown(e);
            gControl.MouseMove += (sender, e) => gControl.Xtragrid_MouseMove(e);
            gView.FocusedRowObjectChanged += (sender, e) => _mview.RefreshBars();
            //gView.SelectionChanged += (sender, e) => _mview.RefreshBars();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            EmailView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _binder = Controller.GetEntityById<Binder>(Text);
            _dataSourceEmailGrid = () => Controller.Collection.OfType<Email>().Where(x => x.Id.Contains(_binder.Id));
            _mview = (MainView)MdiParent;

            LoadEmailGrid(gridControl_email);
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Email), () => gridControl_email.XtraGridRefresh(_dataSourceEmailGrid.Invoke(), saveLayout) }
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            // Ignore
        }
     }
}