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

namespace Flerp.Client.Views
{
    public partial class BasketView : XtraForm, IView
    {
        public BasketView() { InitializeComponent(); }

        private Func<IEnumerable<Document>> _dataSourceDocumentGrid;
        private Func<IEnumerable<Party>> _dataSourcePartyGrid;
        private Func<IEnumerable<Transmittal>> _dataSourceTransmittalGrid;
        private MainView _mview;


        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;

            if (button == _mview.BiProvider.BasketRefresh) Controller.IssueCommand(new BasketRefreshCommand());
            else if (button == _mview.BiProvider.BasketClear) Controller.IssueCommand(new BasketClearCommand());
            else if (button == _mview.BiProvider.TransmittalCreate) Controller.IssueCommand(new CreateTransmittalsCommand());
            else if (button == _mview.BiProvider.TransmittalCommit) Controller.IssueCommand(new CommitTransmittalsCommand());
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.BasketViewBarItems;

            var docs = Controller.Basket.OfType<Document>().Any();
            var parties = Controller.Basket.OfType<Party>().Any() && !Controller.Basket.OfType<Party>().Any(x => string.IsNullOrWhiteSpace(x.TransmittalTarget));
            var trans = Controller.Basket.OfType<Transmittal>().Any();

            (barItems.First(x => x == _mview.BiProvider.TransmittalCommit)).Enabled = !docs & !parties && trans;
            if ((barItems.First(x => x == _mview.BiProvider.TransmittalCommit)).Enabled) splitContainerControl1.SplitterPosition = 0;
            
            (barItems.First(x => x == _mview.BiProvider.TransmittalCreate)).Enabled = docs & parties && !trans;
            (barItems.First(x => x == _mview.BiProvider.BasketRefresh)).Enabled = Controller.Basket.Any();
            (barItems.First(x => x == _mview.BiProvider.BasketClear)).Enabled = Controller.Basket.Any();

            if (!Controller.Basket.OfType<Transmittal>().Any()) splitContainerControl1.SplitterPosition = splitContainerControl1.Width;
            
            return barItems;
        }

        private void BasketView_RestoreLayout()
        {
            splitContainerControl1.SplitterPosition = splitContainerControl1.Width;
            splitContainerControl2.SplitterPosition = splitContainerControl2.Height / 2;

            splitContainerControl1.SizeChanged += splitContainerControl1_SizeChanged;
        }
        
        private void BasketView_SaveLayout()
        {
            var grids = new[] 
            { 
                (GridView)gridControl_document.MainView, 
                (GridView)gridControl_party.MainView, 
                (GridView)gridControl_transmittal.MainView,
            };
            foreach (var view in grids) { view.XtraGridSaveLayout(Name, Controller.AppSettings); }
        }
        
        private void LoadDocumentGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Documents;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Document>(x => x.Privacy, RiProvider.PrivacyEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Document>(x => x.Id, RiProvider.IdEditor(), false, true, 100, 100),
                ColumnDefinition.Create<Document>(x => x.AliasId, RiProvider.TextEditor(), false, false, 225, 225),
                ColumnDefinition.Create<Document>(x => x.Category, RiProvider.CategoryLookupEditor(), false, true, 100, 100),
                ColumnDefinition.Create<Document>(x => x.Name, RiProvider.MemoEditor(), false, false, 300, 300),
                ColumnDefinition.Create<Document>(x => x.Status, RiProvider.LcStatusEditor(), false, true, 27, 27, Resources.Label_Ellipses, Resources.Label_LifeCycleStatus),
                ColumnDefinition.Create<Document>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Document>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gView.ShowingEditor += (sender, e) => gView.Xtragrid_ShowingEditor();
        }

        private void LoadPartyGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Parties;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Party>(x => x.Privacy, RiProvider.PrivacyEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.DbField_Privacy),
                ColumnDefinition.Create<Party>(x => x.Id, RiProvider.IdEditor(), false, true, 50, 50),
                ColumnDefinition.Create<Party>(x => x.Name, RiProvider.TextEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Party>(x => x.Description, RiProvider.TextEditor(), false, false, 175, 175),
                ColumnDefinition.Create<Party>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true, 115, 115),
                ColumnDefinition.Create<Party>(x => x.TransmittalTarget, RiProvider.TransmittalTargetEditor(), false, false, 225, 225),
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);

            gView.ShowingEditor += (sender, e) => gView.Xtragrid_ShowingEditor();
        }

        private void LoadTransmittalGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_TransmittalsDraft;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Transmittal>(x => x.Name, RiProvider.TextEditor(), false, true, 225, 225, Resources.Label_FileSource),
                ColumnDefinition.Create<Transmittal>(x => x.PartyLink, RiProvider.MemoEditor(), false, true, 225, 225, Resources.Label_DownloadLink),
                ColumnDefinition.Create<Transmittal>(x => x.MajorId, RiProvider.MemoEditor(), false, true, 50, 50, Resources.Label_PartyId),
                ColumnDefinition.Create<Transmittal>(x => x.PartyName, RiProvider.MemoEditor(), false, true, 175, 175, Resources.Label_PartyName),
                ColumnDefinition.Create<Transmittal>(x => x.Target, RiProvider.MemoEditor(), false, true, 225, 225, Resources.Label_AddressTarget),                                                            
                ColumnDefinition.Create<Transmittal>(x => x.Notes, RiProvider.MemoEditor(), false, false, 325, 325),
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            BasketView_SaveLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            var msg = string.Format(Resources.Msg_ErrorLoadingView, Text);
            try
            {
                base.OnLoad(e);

                _dataSourceDocumentGrid = () => Controller.Basket.OfType<Document>();
                _dataSourcePartyGrid = () => Controller.Basket.OfType<Party>();
                _dataSourceTransmittalGrid = () => Controller.Basket.OfType<Transmittal>();
                _mview = (MainView)MdiParent;

                LoadDocumentGrid(gridControl_document);
                LoadPartyGrid(gridControl_party);
                LoadTransmittalGrid(gridControl_transmittal);
                BasketView_RestoreLayout();
                
                msg = string.Format(Resources.Msg_SuccessLoadingView, Text);
            }
            finally
            {
                Controller.Logger.Info(msg);
            }
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(Document), () => 
                    { 
                        var docs = _dataSourceDocumentGrid.Invoke();
                        gridControl_document.XtraGridRefresh(docs, saveLayout);
                    } },
                { typeof(Party), () => gridControl_party.XtraGridRefresh(_dataSourcePartyGrid.Invoke(), saveLayout) },
                { typeof(Transmittal), () => 
                    { 
                        var transmittals = _dataSourceTransmittalGrid.Invoke();
                        gridControl_transmittal.XtraGridRefresh(transmittals, saveLayout);
                    } },
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        private void splitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
            splitContainerControl2.SplitterPosition = splitContainerControl2.Height / 2;
        }

        public void ShowEditorByEntity(IPersistable entity) { }
    }
}