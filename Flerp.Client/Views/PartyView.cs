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

namespace Flerp.Client.Views
{
    public partial class PartyView : XtraForm, IView
    {
        public PartyView() { InitializeComponent(); }

        private Party _party;
        private Func<IEnumerable<PartyDetail>> _dataSourcePartyDetail;
        private Func<IEnumerable<PartyParty>> _dataSourcePartyParty;
        private Func<IEnumerable<PartyBinder>> _dataSourcePartyBinder;
        private Func<IEnumerable<Transmittal>> _dataSourceTransmittal;
        private MainView _mview;

        public void BarManager_BarItemClick(object sender, EventArgs e)
        {
            var button = (BarButtonItem)((ItemClickEventArgs)e).Item;
            if (button == _mview.BiProvider.PartyNewPartyDetail && _party != null) Controller.IssueCommand(new NewPartyDetailCommand(_party));

            var transmittal = gridControl_transmittal.XtraGridGetEntity<Transmittal>();
            if (transmittal == null) return;

            if (button == _mview.BiProvider.BasketRefresh) PartyRefresh();
            else if (button == _mview.BiProvider.PartySendTransmittals)
                if (_party != null)
                    Console.WriteLine(Resources.Msg_SendsTransmittalsToX, @"x", _party.Id);
        }

        public object BarManager_GetBarItems()
        {
            var barItems = _mview.BiProvider.PartyViewBarItems;

            (barItems.First(x => x == _mview.BiProvider.PartyRefresh)).Enabled = _dataSourceTransmittal.Invoke().Any();

            return barItems;
        }

        private void _party_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            propertyGridControl_party.Refresh();
        }

        private void PartyView_RestoreLayout()
        {
            var layout = Controller.AppSettings.GetById(Resources.Layout_Prefix + Name);
            if (layout == null) return;

            var obj = JsonConvert.DeserializeAnonymousType(layout.Value, new { arg1 = 99, arg2 = 99 }).ThrowIfNull();

            splitContainerControl1.SplitterPosition = obj.arg1;
            splitContainerControl2.SplitterPosition = obj.arg2;
        }

        private void PartyView_SaveLayout()
        {
            var grids = new[] 
            { 
                (GridView)gridControl_partyDetail.MainView,
                (GridView)gridControl_partyParty.MainView, 
                (GridView)gridControl_partyBinder.MainView,
                (GridView)gridControl_transmittal.MainView,
            };
            foreach (var view in grids) { view.XtraGridSaveLayout(Name, Controller.AppSettings); }

            var json = JsonConvert.SerializeObject(
                new
                {
                    arg1 = splitContainerControl1.SplitterPosition,
                    arg2 = splitContainerControl2.SplitterPosition,
                }).ThrowIfNull();

            Controller.AppSettings.Save(new AppSetting(Resources.Layout_Prefix + Name, json));
        }

        private void LoadPartyGrid(PropertyGridControl gControl)
        {
            gControl.XtraVerticalGridInitializeRows(new[]
            {
                ColumnDefinition.Create<Party>(x => x.Name, RiProvider.MemoEditor(), false, false),
                ColumnDefinition.Create<Party>(x => x.Privacy, RiProvider.PrivacyEditorPg(), false, false),
                ColumnDefinition.Create<Party>(x => x.Description, RiProvider.MemoEditor(), false, false),
                ColumnDefinition.Create<Party>(x => x.ModifiedDate, RiProvider.DateEditor(), false, true),
                ColumnDefinition.Create<Party>(x => x.CreatedDate, RiProvider.DateEditor(), false, true),
                ColumnDefinition.Create<Party>(x => x.DisposedDate, RiProvider.DateEditor(), true, true, 115, 115, Resources.Label_Deleted)       
            });

            gControl.XtraVerticalGridSetOptions();
            gControl.XtraVerticalGridPopulate(_party);

            gControl.CellValueChanged += (sender, e) => _party.Save();
            _party.PropertyChanged += _party_PropertyChanged;
        }
        
        private void LoadPartyDetailGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Term_Details;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyDetail>(x => x.DetailType, RiProvider.DetailTypeEditor(), false, false, 27, 27, Resources.Label_Ellipses, Resources.Label_Type),
                ColumnDefinition.Create<PartyDetail>(x => x.Name, RiProvider.TextEditor(), false, false, 120, 120, Resources.DbField_Description),
                ColumnDefinition.Create<PartyDetail>(x => x.MinorId, RiProvider.MemoEditor(), false, false, 250, 250, Resources.Label_Value),
                ColumnDefinition.Create<PartyDetail>(x => x.Id, RiProvider.DeleteButtonEditor(), false, true, 32, 32, Resources.Label_Ellipses, Resources.Label_Delete)
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }

        private void LoadPartyPartyGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_RelatedParties;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyParty>(x => x.MajorId, RiProvider.IdEditor(), false, true, 50, 50, Resources.DbField_Id +  "_1"),
                ColumnDefinition.Create<PartyParty>(x => x.MajorName, RiProvider.TextEditor(), false, true, 200, 200, Resources.DbField_Name + "_1"),
                ColumnDefinition.Create<PartyParty>(x => x.MinorId, RiProvider.IdEditor(), false, true, 50, 50, Resources.DbField_Id + "_2"),
                ColumnDefinition.Create<PartyParty>(x => x.MinorName, RiProvider.TextEditor(), false, true, 200, 200, Resources.DbField_Name + "_2"),
                ColumnDefinition.Create<PartyParty>(x => x.Name, RiProvider.TextEditor(), false, false, 300, 300, Resources.Label_Relationship),
                ColumnDefinition.Create<PartyParty>(x => x.Id, RiProvider.DeleteButtonEditor(), false, true, 32, 32,  Resources.Label_Ellipses, Resources.Label_Delete)
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }

        private void LoadPartyBinderGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_Binders;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<PartyBinder>(x => x.MinorId, RiProvider.IdEditor(), false, true, 50, 50, Resources.DbField_Id),
                ColumnDefinition.Create<PartyBinder>(x => x.BinderName, RiProvider.TextEditor(), false, true, 175, 175, Resources.DbField_Name),
                ColumnDefinition.Create<PartyBinder>(x => x.Name, RiProvider.TextEditor(), false, false, 175, 175, Resources.Label_Role),
                ColumnDefinition.Create<PartyBinder>(x => x.Id, RiProvider.DeleteButtonEditor(), false, true, 32, 32, Resources.Label_Ellipses, Resources.Label_Delete)
            });

            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }

        private void LoadTransmittalGrid(GridControl gControl)
        {
            var gView = (GridView)gControl.MainView;
            gView.ViewCaption = Resources.Label_Transmittals;

            gView.XtragridInitializeColumns(new[]
            {
                ColumnDefinition.Create<Transmittal>(x => x.Name, RiProvider.TextEditor(), false, true, 225, 225, Resources.Label_FileSource),
                ColumnDefinition.Create<Transmittal>(x => x.CreatedDate, RiProvider.DateEditor(), true, true, 115, 115),
                ColumnDefinition.Create<Transmittal>(x => x.DocName, RiProvider.MemoEditor(), false, true, 300, 300),
                //ColumnDefinition.Create<Transmittal>(x => x.PartyLink, RiProvider.TransmittalLinkEditor(), false, true, 150, 150, Resources.Label_Url),
                ColumnDefinition.Create<Transmittal>(x => x.Target, RiProvider.MemoEditor(), false, true, 225, 225, Resources.Label_AddressTarget), 
                ColumnDefinition.Create<Transmittal>(x => x.DocAliasId, RiProvider.TextEditor(), false, true, 225, 225),                                           
                ColumnDefinition.Create<Transmittal>(x => x.Notes, RiProvider.MemoEditor(), false, true, 325, 325)
            });
            
            gView.XtragridSetOptions(false);
            gView.XtraGridRestoreLayout(Name, Controller.AppSettings);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            PartyView_SaveLayout();
            _party.PropertyChanged -= _party_PropertyChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _party = Controller.GetEntityById<Party>(Text);
            _dataSourcePartyDetail = () => Controller.Collection.OfType<PartyDetail>().Where(x => x.MajorId == _party.Id);
            _dataSourcePartyParty = () => Controller.Collection.OfType<PartyParty>().Where(x => x.MajorId == _party.Id || x.MinorId == _party.Id);
            _dataSourcePartyBinder = () => Controller.Collection.OfType<PartyBinder>().Where(x => x.MajorId == _party.Id);
            _dataSourceTransmittal = () => Controller.Collection.OfType<Transmittal>().Where(x => x.MajorId == _party.Id);
            _mview = (MainView)MdiParent;

            LoadPartyGrid(propertyGridControl_party);
            LoadPartyDetailGrid(gridControl_partyDetail);
            LoadPartyPartyGrid(gridControl_partyParty);
            LoadPartyBinderGrid(gridControl_partyBinder);
            LoadTransmittalGrid(gridControl_transmittal);
            PartyView_RestoreLayout();
        }

        private void PartyRefresh()
        {
            RefreshView(true);
            _mview.RefreshBars();
        }

        public void RefreshView(bool saveLayout, Type[] types = null)
        {
            var refresh = new Dictionary<Type, Action>
            {
                { typeof(PartyDetail),  () => gridControl_partyDetail.XtraGridRefresh(_dataSourcePartyDetail.Invoke(), saveLayout) },
                { typeof(PartyParty),   () => gridControl_partyParty.XtraGridRefresh(_dataSourcePartyParty.Invoke(), saveLayout) },
                { typeof(PartyBinder),  () => gridControl_partyBinder.XtraGridRefresh(_dataSourcePartyBinder.Invoke(), saveLayout) },
                { typeof(Transmittal),  () => gridControl_transmittal.XtraGridRefresh(_dataSourceTransmittal.Invoke(), saveLayout) },
            };

            if (types == null) foreach (var item in refresh) item.Value.Invoke();
            else foreach (var type in new HashSet<Type>(types).Where(type => refresh.ContainsKey(type))) refresh[type].Invoke();

            _mview.PerformSafely(() => _mview.RefreshBars());
        }

        public void ShowEditorByEntity(IPersistable entity)
        {
            GridView gView = null;

            if (entity is PartyDetail) gView = (GridView)gridControl_partyDetail.MainView;
            if (gView != null) gView.XtraGridShowEditor(entity, null);
        }
     }
}