using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraTabbedMdi.Helpers;
using Flerp.Client.Helpers;
using Flerp.Client.Properties;
using Flerp.DomainModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flerp.Client.Views
{
    public partial class MainView : XtraForm, IMainView
    {
        public MainView() 
        { 
            InitializeComponent();

            var DdHelper = new DragDropHelper(xtraTabbedMdiManager1);

            Controller.Collection.ListChanged += Collection_ListChanged;
            Controller.Basket.ListChanged += Collection_ListChanged;

            xtraTabbedMdiManager1.SelectedPageChanged += SelectedPageChanged;
        }


        public IView ActiveView
        {
            get
            {
                IView view = null;

                var page = xtraTabbedMdiManager1.SelectedPage;
                if (page != null) view = (IView)page.MdiChild;

                return view ?? ((IView) xtraTabbedMdiManager1.ActiveFloatForm);
            }
        }

        private Timer BarManagerTimer { get; set; }

        public BarItemProvider BiProvider { get; private set; }
             
   
        private Bar BarManager_GetStatusBar()
        {
            var bar = new Bar(barManager1, Resources.Name_StatusBar);
            bar.AddItem(BiProvider.StatusCollectionStats);
            return bar;
        }

        private Bar BarManager_GetStandaloneBar(string name, BarItem[] barItems)
        {
            var bar = new Bar(barManager1, name)
            {
                DockStyle = BarDockStyle.Standalone,
                StandaloneBarDockControl = standaloneBarDockControl1
            };
            bar.OptionsBar.AllowQuickCustomization = false;
            bar.OptionsBar.DrawDragBorder = false;
            if (barItems != null) bar.AddItems(barItems);

            return bar;
        }

        private void BarManager_Initialize()
        {
            BiProvider = new BarItemProvider(barManager1, standaloneBarDockControl1);

            barManager1.AllowCustomization = false;
            barManager1.AllowQuickCustomization = false;
            barManager1.AllowShowToolbarsPopup = false;

            var sbar = BarManager_GetStatusBar();
            barManager1.Bars.Add(sbar);
            barManager1.StatusBar = sbar;

            BarManagerTimer = new Timer { Interval = 100 };
            BarManagerTimer.Tick += BarManager_Timer_Tick;
            BarManagerTimer.Start();

            barManager1.ItemClick += BarManager_ItemClick;
        }
        
        private void BarManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ActiveView.ThrowIfNull().BarManager_BarItemClick(sender, e);
            }
            catch
            {
                Console.WriteLine(Resources.Msg_ButtonClickError);
            }
        }

        private void BarManager_Timer_Tick(object sender, EventArgs e)
        {
            var biStats = barManager1.Items[Controller.Collection.Name];
            if (biStats == null) return;

            barManager1.BeginUpdate();
            this.PerformSafely(() => biStats.Caption = Controller.GetStats());
            barManager1.EndUpdate();
        }

        private void Collection_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (Controller.Queues[WqType.Q2].Count > 1) return;

                var collection = ((IBindingList)sender).Cast<IPersistable>();

                if (ActiveView == null) return;

                if (e.ListChangedType == ListChangedType.Reset || e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    ActiveView.RefreshView(true);
                }

                else if (e.ListChangedType == ListChangedType.ItemChanged)
                {
                    ActiveView.RefreshView(true, new[] { collection.ElementAt(e.NewIndex).GetType() });
                }

                else if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    var persistables = collection as IPersistable[] ?? collection.ToArray();
                    ActiveView.RefreshView(true, new[] { persistables.ElementAt(e.NewIndex).GetType() });
                    ActiveView.ShowEditorByEntity(persistables.ElementAt(e.NewIndex));
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        private XtraMdiTabPage IsPageExisting(string id)
        {
            var dockedPage = xtraTabbedMdiManager1
                .Pages
                .Cast<XtraMdiTabPage>()
                .FirstOrDefault(page => id == page.Text);
            if (dockedPage != null) return dockedPage;

            var floatForm = xtraTabbedMdiManager1
                .FloatForms
                .FirstOrDefault(x => x.Name == id);
            return floatForm != null ? xtraTabbedMdiManager1.Pages[floatForm] : null;
        }

        protected async override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var exitMsg = Resources.Msg_OkToExitApp;
            var qSave = false;

            if (Controller.GetQueueStatus() != WqStatus.Idle)
            {
                exitMsg = Resources.Msg_OutstandingTasks;
                qSave = true;
            }
            var dialogResult = MessageBox.Show(exitMsg, Resources.Label_ApplicationExit, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }

            if (!qSave) return;
            var saves = Controller.Queues.Select(q => q.Value.Save());
            await Task.WhenAll(saves);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BarManager_Initialize();           

            Show_FixedView<BasketView>(Resources.Label_Basket);
            Show_FixedView<HomeView>(Resources.Label_Home);
        }

        public void RefreshBars()
        {
            barManager1.BeginUpdate();
            var newBar = BarManager_GetStandaloneBar(ActiveView.Name, (BarItem[])ActiveView.BarManager_GetBarItems());

            for (var i = 0; i < barManager1.Bars.Count; i++)
            {
                if (barManager1.Bars[i].BarName != Resources.Name_StatusBar) barManager1.Bars.RemoveAt(i);
            }

            barManager1.Bars.Add(newBar);
            barManager1.MainMenu = newBar;
            barManager1.EndUpdate();
        }

        private void SelectedPageChanged(object sender, EventArgs e)
        {
            ActiveView.RefreshView(true);
        }

        public void Show_AppSettingsView()
        {
            var page = IsPageExisting(Resources.Caption_AppSettingsView);
            if (page == null)
            {
                Form v = new AppSettingsView { Text = Resources.Caption_AppSettingsView, MdiParent = this };
                v.Show();
                return;
            }
            xtraTabbedMdiManager1.SelectedPage = page;
        }

        public void Show_DocView()
        {
            var page = IsPageExisting(Resources.Caption_DocView);
            if (page == null)
            {
                Form v = new DocView { Text = Resources.Caption_DocView, MdiParent = this };
                v.Show();
                return;
            }
            xtraTabbedMdiManager1.SelectedPage = page;
        }

        public void Show_StubView()
        {
            var page = IsPageExisting(Resources.Caption_StubView);
            if (page == null)
            {
                Form v = new StubView { Text = Resources.Caption_StubView, MdiParent = this };
                v.Show();
                return;
            }
            xtraTabbedMdiManager1.SelectedPage = page;
        }

        public Form Show_BinderOrPartyView(BinderBase binder)
        {
            var page = IsPageExisting(binder.Id);
            if (page == null)
            {
                Form v = null;
                var c = binder.Id[0];

                if (c == 'A' || c == 'W')   v = new BinderView { Text = binder.Id, MdiParent = this };
                if (c == 'E')               v = new EmailView { Text = binder.Id, MdiParent = this };
                if (c == 'L')               v = new LibraryView { Text = binder.Id, MdiParent = this };
                if (c == 'H' || c == 'O')   v = new PartyView { Text = binder.Id, MdiParent = this };
                if (v != null) v.Show();
                return v;
            }
            xtraTabbedMdiManager1.SelectedPage = page;
            return page.MdiChild;
        }

        public Form Show_FixedView<T>(string name) where T : Form, new()
        {
            var page = IsPageExisting(name);
            if (page == null)
            {
                var v = new T { Text = name, MdiParent = this };
                page = xtraTabbedMdiManager1.XtraMdiTabGet(v.Text);
                if (page != null) page.ShowCloseButton = DefaultBoolean.False;
                v.Show();
                return v;
            }

            xtraTabbedMdiManager1.SelectedPage = page;
            return page.MdiChild;
        }

    }
}