using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Flerp.Client.Helpers;
using Flerp.DomainModel;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace Flerp.Client.Views
{
    public partial class AppSettingsView : XtraForm, IView
    {
        private MainView _mview;
        private BindingList<EmailAccount> Accounts { get; set; }
        private BindingList<Category> Categories { get; set; }

        public AppSettingsView() { InitializeComponent(); }

        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;
            if (button == _mview.BiProvider.SaveChanges) SetConfig();
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.AppSettingsViewBarItems;

            (barItems.First(x => x == _mview.BiProvider.SaveChanges)).Enabled = true;

            return barItems;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _mview = (MainView)MdiParent;

            GetConfig();
            RefreshView(false);
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity) { }

        private void GetConfig()
        {
            te_LocalDb1.Text = ConfigurationManager.AppSettings["ConnectionString"];
            te_LocalDb2.Text = ConfigurationManager.AppSettings["DbServerPath"];
            te_LocalDb3.Text = ConfigurationManager.AppSettings["DbServerArgs"];

            te_CloudDb1.Text = ConfigurationManager.AppSettings["CloudServiceProvider"];
            te_CloudDb2.Text = ConfigurationManager.AppSettings["CloudAccountName"];
            te_CloudDb3.Text = ConfigurationManager.AppSettings["CloudAccountKey"];

            te_DownloadCap.Text = ConfigurationManager.AppSettings["EmailDownloadCap"];

            // Retrieve EmailAccounts
            Accounts = new BindingList<EmailAccount>();
            Accounts.AllowNew = true;
            Accounts.AllowEdit = true;
            Accounts.AllowRemove = true;
            gridControl_emailAccount.DataSource = Accounts;

            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("EmailAccount_")))
                Accounts.Add(EmailAccount.Parse(ConfigurationManager.AppSettings[key]));

            // Retrieve Categories
            Categories = new BindingList<Category>();
            Categories.AllowNew = true;
            Categories.AllowEdit = true;
            Categories.AllowRemove = true;
            gridControl_category.DataSource = Categories;

            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("Category_")))
                Categories.Add(Category.Parse(ConfigurationManager.AppSettings[key]));
        }

        private void SetConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["ConnectionString"].Value = te_LocalDb1.Text;
            config.AppSettings.Settings["DbServerPath"].Value = te_LocalDb2.Text;
            config.AppSettings.Settings["DbServerArgs"].Value = te_LocalDb3.Text;
            config.AppSettings.Settings["CloudServiceProvider"].Value = te_CloudDb1.Text;
            config.AppSettings.Settings["CloudAccountName"].Value = te_CloudDb2.Text;
            config.AppSettings.Settings["CloudAccountKey"].Value = te_CloudDb3.Text;
            config.AppSettings.Settings["EmailDownloadCap"].Value = te_DownloadCap.Text;

            // Retrieve EmailAccounts
            var i = 0;
            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("EmailAccount_"))) config.AppSettings.Settings.Remove(key);
            foreach (var account in Accounts) config.AppSettings.Settings.Add("EmailAccount_" + i++, account.Serialize());

            // Retrieve Categories
            i = 0;
            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("Category_"))) config.AppSettings.Settings.Remove(key);
            foreach (var category in Categories) config.AppSettings.Settings.Add("Category_" + i++, category.Serialize());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Controller.Logger.Info("AppSettings saved.");
        }
    }
}