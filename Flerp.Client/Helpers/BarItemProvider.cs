using DevExpress.Utils;
using DevExpress.XtraBars;
using Flerp.Client.Properties;
using System.Drawing;

namespace Flerp.Client.Helpers
{
    public class BarItemProvider
    {
        private readonly BarManager _manager;

        public BarItem BasketClear { get; private set; }
        public BarItem BasketRefresh { get; private set; }
        public BarItem BinderNewAdmin { get; private set; }
        public BarItem BinderNewWork { get; private set; }
        public BarItem DocumentCancel { get; private set; }
        public BarItem DocumentConvertToDirectory { get; private set; }
        public BarItem DocumentConvertToFile { get; private set; }
        public BarItem DocumentNewNull { get; private set; }
        public BarItem DocumentNewCopy { get; private set; }
        public BarItem DocumentNewRevision { get; private set; }
        public BarItem DocumentRelease { get; private set; }
        public BarItem DocView { get; private set; }
        public BarItem EmailCancel { get; private set; }
        public BarItem EmailGet { get; private set; }
        private BarItem EmailImportReceived { get; set; }
        private BarItem EmailImportSent { get; set; }
        public BarItem PartyNewHuman { get; private set; }
        public BarItem PartyNewOrganisation { get; private set; }
        public BarItem PartyNewPartyDetail { get; private set; }
        public BarItem PartyRefresh { get; private set; }
        public BarItem PartySendTransmittals { get; private set; }
        public BarItem StatusCollectionStats { get; private set; }
        public BarItem StubNew { get; private set; }
        public BarItem StubView { get; private set; }
        public BarItem TransmittalCommit { get; private set; }
        public BarItem TransmittalCreate { get; private set; }
        public BarItem AppSettings { get; private set; }
        public BarItem SaveChanges { get; private set; }
        public BarItem DiscardChanges { get; private set; }

        public BarItem[] AppSettingsViewBarItems
        {
            get
            {
                return new[] 
                { 
                    SaveChanges
                };
            }
        }
        public BarItem[] BasketViewBarItems
        {
            get
            {
                return new[] 
                { 
                    TransmittalCreate,
                    TransmittalCommit, 
                    BasketRefresh, 
                    BasketClear,
                };
            }
        }
        public BarItem[] BinderViewBarItems
        {
            get
            {
                return new[] 
                { 
                    StubNew,
                    DocumentNewNull, 
                    DocumentNewCopy, 
                    DocumentNewRevision,
                    DocumentRelease,
                    DocumentCancel, 
                    DocumentConvertToDirectory,
                    DocumentConvertToFile,
                };
            }
        }
        public BarItem[] HomeViewBarItems
        {
            get
            {
                return new[] 
                { 
                    BinderNewAdmin, 
                    BinderNewWork,
                    PartyNewHuman, 
                    PartyNewOrganisation, 
                    PartyNewPartyDetail,
                    StubView,
                    DocView,
                    EmailGet,
                    AppSettings,
                };
            }
        }
        public BarItem[] EmailViewBarItems
        {
            get
            {
                return new[] 
                { 
                    StubNew,
                    EmailGet,
                    EmailImportReceived,
                    EmailImportSent,
                    EmailCancel,
                };
            }
        }
        public BarItem[] LibraryViewBarItems
        {
            get
            {
                return new[] 
                { 
                    StubNew,
                    DocumentNewNull, 
                    DocumentNewCopy, 
                    DocumentNewRevision,
                    DocumentRelease,
                    DocumentCancel, 
                    DocumentConvertToDirectory,
                    DocumentConvertToFile
                };
            }
        }
        public BarItem[] PartyViewBarItems
        {
            get
            {
                return new[] 
                {
                    PartyNewPartyDetail,
                    PartySendTransmittals,
                    PartyRefresh,
                };
            }
        }
        public BarItem[] StubViewBarItems
        {
            get { return null; }
        }
        public BarItem[] DocViewBarItems
        {
            get { return null; }
        }


        public BarItemProvider(BarManager manager, StandaloneBarDockControl standalone)
        {
            _manager = manager;

            BasketClear                = GetButton(Resources.Name_BasketClear, Resources.Caption_BasketClear, string.Empty, Resources.round_delete_icon_24);
            BasketRefresh              = GetButton(Resources.Name_BasketRefresh, Resources.Label_Refresh, string.Empty, Resources.refresh_icon_24);
            BinderNewAdmin             = GetButton(Resources.Name_BinderNewAdmin, Resources.Caption_BinderNewAdmin, Resources.Tooltip_BinderNewAdmin, Resources.folderA_plus_icon_24);
            BinderNewWork              = GetButton(Resources.Name_BinderNewWork, Resources.Caption_BinderNewWork, string.Empty, Resources.folderW_plus_icon_24);
            DocumentCancel             = GetButton(Resources.Name_DocumentCancel, Resources.Caption_DocumentBaseCancel, string.Empty, Resources.doc_export_icon_24);
            DocumentConvertToDirectory = GetButton(Resources.Name_DocumentConvertToDirectory, Resources.Caption_DocumentConvertToDirectory, string.Empty, Resources.folder_icon_24);
            DocumentConvertToFile      = GetButton(Resources.Name_DocumentConvertToFile, Resources.Caption_DocumentConvertToFile, string.Empty, Resources.doc_empty_icon_24);
            DocumentNewNull            = GetButton(Resources.Caption_DocumentNewNull, Resources.Caption_DocumentNewNull, string.Empty, Resources.doc_new_icon_24);
            DocumentNewCopy            = GetButton(Resources.Name_DocumentNewCopy, Resources.Caption_DocumentNewCopy, string.Empty, Resources.doc_new_icon_24);
            DocumentNewRevision        = GetButton(Resources.Name_DocumentNewRevision, Resources.Caption_DocumentNewRevision, string.Empty, Resources.doc_plus_icon_24);
            DocumentRelease            = GetButton(Resources.Name_DocumentRelease, Resources.Caption_DocumentRelease, string.Empty, Resources.doc_export_icon_24);
            DocView                    = GetButton(Resources.Name_DocView, Resources.Caption_DocView, string.Empty, Resources.doc_new_icon_24);
            EmailCancel                = GetButton(Resources.Name_EmailCancel, Resources.Caption_DocumentBaseCancel, string.Empty, Resources.doc_export_icon_24);
            EmailGet                   = GetButton(Resources.Name_EmailGet, Resources.Caption_EmailGet, string.Empty, Resources.download_icon_24);
            EmailImportReceived        = GetButton(Resources.Name_EmailImportReceived, Resources.Label_Received, string.Empty, Resources.import_icon_24);
            EmailImportSent            = GetButton(Resources.Name_EmailImportSent, Resources.Caption_EmailImportSent, string.Empty, Resources.export_icon_24);
            PartyNewHuman              = GetButton(Resources.Name_PartyNewHuman, Resources.Caption_PartyNewHuman, string.Empty, Resources.user_add_icon_24);
            PartyNewOrganisation       = GetButton(Resources.Name_PartyNewOrganisation, Resources.Caption_PartyNewOrganisation, string.Empty, Resources.user_add_icon_24);
            PartyNewPartyDetail        = GetButton(Resources.Name_PartyNewPartyDetail, Resources.Caption_PartyNewPartyDetail, string.Empty, Resources.contact_add_icon_24);
            PartyRefresh               = GetButton(Resources.Name_PartyRefresh, Resources.Label_Refresh, string.Empty, Resources.refresh_icon_24);
            PartySendTransmittals      = GetButton(Resources.Name_PartySendTransmittals, Resources.Caption_PartySendTransmittals, string.Empty, Resources.mail_icon_24);
            StubNew                    = GetButton(Resources.Name_StubNew, Resources.Caption_StubNew, string.Empty, Resources.list_num_icon_24);
            StubView                   = GetButton(Resources.Name_StubView, Resources.Caption_StubView, string.Empty, Resources.list_num_icon_24);
            TransmittalCommit          = GetButton(Resources.Name_TransmittalCommit, Resources.Caption_TransmittalCommit, string.Empty, Resources.round_checkmark_icon_24);
            TransmittalCreate          = GetButton(Resources.Name_TransmittalCreate, Resources.Caption_TransmittalCreate, string.Empty, Resources.round_arrow_down_icon_24);

            AppSettings                = GetButton(Resources.Name_AppSettings, Resources.Caption_AppSettings, string.Empty, Resources.wrench_icon_24);
            SaveChanges                = GetButton(Resources.Name_SaveChanges, Resources.Caption_SaveChanges, string.Empty, Resources.round_checkmark_icon_24);

            StatusCollectionStats      = GetStaticGlyphItem(Controller.Collection.Name, Controller.GetStats(), string.Empty, Resources.db_copy_icon_16);
        }
               
        private BarItem GetButton(string name, string caption, string hint, Image image)
        {
            var bi = new BarLargeButtonItem
            {
                Name = name,
                Manager = _manager,
                Caption = caption,
                Hint = hint,
                Glyph = image,
                PaintStyle = BarItemPaintStyle.CaptionGlyph,
                AllowGlyphSkinning = DefaultBoolean.True,
                CaptionAlignment = BarItemCaptionAlignment.Bottom,
                Enabled = false,
            };

            return bi;
        }

        private BarItem GetStaticGlyphItem(string name, string caption, string hint, Image image)
        {
            var bi = new BarStaticItem
            {
                Name = name,
                Manager = _manager,
                Caption = caption,
                Hint = hint,
                Alignment = BarItemLinkAlignment.Right,
                Glyph = image,
                PaintStyle = BarItemPaintStyle.CaptionGlyph,
                AllowGlyphSkinning = DefaultBoolean.True,
            };
            return bi;
        }
    }
}