using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using Flerp.Client.Properties;
using Flerp.DomainModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Flerp.Client.Helpers
{
    public static class RiProvider
    {
        private static Func<IEnumerable<Party>> DataSourcePartyGrid { get { return () => Controller.Collection.OfType<Party>(); } }

        public static RepositoryItemComboBox CategoryLookupEditor()
        {
            var ri = new RepositoryItemComboBox
            {
                Name = Resources.DbField_Category + Resources.Label_Editor,
                CycleOnDblClick = true,
                DropDownRows = 50
            };
            ri.Items.Clear();
            return ri;
        }
        public static void RefreshCategoryLookupEditor(GridView gView, GridColumn col)
        {
            var id = ((IPersistable)gView.GetFocusedRow()).Id;
            var code = Regex.Replace(id, Resources.Regex_FlerpIdLettersOnly, string.Empty);
            if (string.IsNullOrWhiteSpace(code)) return;

            var items = Category.GetAll()
                .Where(x => x.Permissions.Contains(code))
                .Select(x => x.Name)
                .ToList();

            var editor = (RepositoryItemComboBox)col.ColumnEdit;
            editor.Items.Clear();
            editor.Items.AddRange(items);
        }

        public static RepositoryItemTextEdit CurrencyEditor()
        {
            var ri = new RepositoryItemTextEdit { Name = Resources.Label_Currency + Resources.Label_Editor };
            ri.Mask.MaskType = MaskType.Numeric;
            ri.DisplayFormat.FormatType = FormatType.Numeric;
            ri.Mask.EditMask = @"c";
            ri.DisplayFormat.FormatString = "c2";
            return ri;
        }
        public static RepositoryItemDateEdit DateEditor()
        {
            var ri = new RepositoryItemDateEdit { Name = Resources.Label_Date + Resources.Label_Editor };
            ri.Mask.UseMaskAsDisplayFormat = true;
            ri.EditMask = "g";
            ri.CalendarTimeEditing = DefaultBoolean.True;
            return ri;
        }
        public static RepositoryItemButtonEdit DeleteButtonEditor()
        {
            var ri = new RepositoryItemButtonEdit { Name = Resources.Label_DeleteButton + Resources.Label_Editor };
            ri.Buttons.Clear();
            ri.Buttons.Add(new EditorButton(ButtonPredefines.Glyph, Resources.round_delete_icon_16, null));
            ri.TextEditStyle = TextEditStyles.HideTextEditor;
            return ri;
        }
        public static RepositoryItemImageComboBox DetailTypeEditor()
        {
            var items = new Dictionary<EnumEx, Image> 
                {
                    { PartyDetailType.Address, Resources.home_icon_16 },
                    { PartyDetailType.Email, Resources.mail_icon_16 },
                    { PartyDetailType.Misc, Resources.notepad_2_icon_16 },
                    { PartyDetailType.Phone, Resources.phone_2_icon_16 },
                    { PartyDetailType.Web, Resources.globe_1_icon_16 },
                };

            var ri = new RepositoryItemImageComboBox { Name = Resources.Label_DetailType + Resources.Label_Editor };
            var ic = new ImageCollection();
            for (int i = 0; i < items.Count; i++)
            {
                ic.AddImage(items.ElementAt(i).Value);
                ri.Items.Add(new ImageComboBoxItem(items.ElementAt(i).Key.DisplayName, items.ElementAt(i).Key, i));
            }
            ri.SmallImages = ic;
            ri.GlyphAlignment = HorzAlignment.Center;
            ri.Buttons.Clear();
            return ri;
        }
        public static RepositoryItemHyperLinkEdit IdEditor()
        {
            var ri = new RepositoryItemHyperLinkEdit { Name = Resources.DbField_Id + Resources.Label_Editor };
            return ri;
        }
        public static RepositoryItemTextEdit IntegerEditor()
        {
            var ri = new RepositoryItemTextEdit { Name = Resources.Label_Integer + Resources.Label_Editor };
            ri.Mask.MaskType = MaskType.Numeric;
            ri.DisplayFormat.FormatType = FormatType.Numeric;

            return ri;
        }
        public static RepositoryItemCheckEdit IsClickedEditor()
        {
            var ri = new RepositoryItemCheckEdit
            {
                Name = Resources.Label_IsClicked + Resources.Label_Editor,
                CheckStyle = CheckStyles.UserDefined
            };


            var ic = new ImageCollection();
            ic.AddImage(Resources.round_checkmark_icon_16);
            ic.AddImage(Resources.blank_icon_16);

            ri.Images = ic;
            ri.ImageIndexChecked = 0;
            ri.ImageIndexUnchecked = 1;

            return ri;
        }
        public static RepositoryItemCheckEdit IsCompleteEditor()
        {
            var ri = new RepositoryItemCheckEdit
            {
                Name = Resources.Label_IsComplete + Resources.Label_Editor,
                CheckStyle = CheckStyles.UserDefined,
                AllowGrayed = true
            };

            var ic = new ImageCollection();
            ic.AddImage(Resources.checkbox_checked_icon_16);
            ic.AddImage(Resources.checkbox_unchecked_icon_16);
            ic.AddImage(Resources.blank_icon_16);

            ri.Images = ic;
            ri.ImageIndexChecked = 0;
            ri.ImageIndexUnchecked = 1;
            ri.ImageIndexGrayed = 2;

            return ri;
        }
        public static RepositoryItemCheckEdit IsIgnorableEditor()
        {
            var ri = new RepositoryItemCheckEdit
            {
                Name = Resources.Label_IsIgnorable + Resources.Label_Editor,
                CheckStyle = CheckStyles.UserDefined
            };

                        var ic = new ImageCollection();
            ic.AddImage(Resources.mailred_icon_16);
            ic.AddImage(Resources.mailblue_icon_16);

            ri.Images = ic;
            ri.ImageIndexChecked = 0;
            ri.ImageIndexUnchecked = 1;

            return ri;
        }
        public static RepositoryItemCheckEdit IsPostedEditor()
        {
            var ri = new RepositoryItemCheckEdit
            {
                Name = Resources.Label_IsPosted + Resources.Label_Editor,
                CheckStyle = CheckStyles.UserDefined
            };

            var ic = new ImageCollection();
            ic.AddImage(Resources.pin_map_icon_16);
            ic.AddImage(Resources.blank_icon_16);

            ri.Images = ic;
            ri.ImageIndexChecked = 0;
            ri.ImageIndexUnchecked = 1;

            return ri;
        }
        public static RepositoryItemImageComboBox LcStatusEditor()
        {
            var ri = new RepositoryItemImageComboBox
            {
                Name = Resources.DbField_LcStatus + Resources.Label_Editor,
                GlyphAlignment = HorzAlignment.Center
            };

            var items = new Dictionary<EnumEx, Image> 
                {
                    { LcStatusType.Pending, Resources.pending_icon_16 },
                    { LcStatusType.Released, Resources.released_icon_16 },
                    { LcStatusType.Cancelled, Resources.cancelled_icon_16 },
                };

            var ic = new ImageCollection();
            
            for (var i = 0; i < items.Count; i++)
            {
                ic.AddImage(items.ElementAt(i).Value);
                ri.Items.Add(new ImageComboBoxItem(items.ElementAt(i).Key.DisplayName, items.ElementAt(i).Key, i));
            }
            ri.SmallImages = ic;
            ri.Buttons.Clear();
            return ri;
        }
        public static RepositoryItemMemoEdit MemoEditor()
        {
            var ri = new RepositoryItemMemoEdit
            {
                Name = Resources.Label_Memo + Resources.Label_Editor,
                AcceptsReturn = false
            };
            ri.KeyDown += TextEditor_KeyDown;
            return ri;
        }
        public static RepositoryItemMemoExEdit MemoExEditor()
        {
            var ri = new RepositoryItemMemoExEdit
            {
                Name = Resources.Label_MemoEx + Resources.Label_Editor,
                ShowIcon = false
            };
            return ri;
        }
        public static RepositoryItemMemoExEdit MemoPrivateEditor()
        {
            var ri = new RepositoryItemMemoExEdit
            {
                Name = Resources.Label_MemoPrivate + Resources.Label_Editor,
                ShowDropDown = ShowDropDown.SingleClick
            };
            ri.Buttons.Clear();
            return ri;
        }
        public static RepositoryItemLookUpEdit PartyLookupEditor()
        {
            var ri = new RepositoryItemLookUpEdit
            {
                Name = Resources.Term_Parties + Resources.Label_Editor,
                DataSource = DataSourcePartyGrid.Invoke(),
                ValueMember = Resources.DbField_Id,
                DisplayMember = Resources.DbField_Name,
                BestFitMode = BestFitMode.BestFitResizePopup,
                SearchMode = SearchMode.AutoComplete,
                AutoSearchColumnIndex = 1,
                NullText = Resources.Label_Unassigned,
                DropDownRows = 50,
                UseDropDownRowsAsMaxCount = true,
            };
            ri.Columns.Add(new LookUpColumnInfo(Resources.DbField_Id, 0));
            ri.Columns.Add(new LookUpColumnInfo(Resources.DbField_Name, 1));
            return ri;
        }
        public static RepositoryItemImageComboBox PrivacyEditor()
        {
            var ri = new RepositoryItemImageComboBox
            {
                Name = Resources.DbField_Privacy + Resources.Label_Editor,
                GlyphAlignment = HorzAlignment.Center
            };

            var items = new Dictionary<EnumEx, Image> 
                {
                    { PrivacyType.Default, Resources.blank_icon_16 },
                    { PrivacyType.Share, Resources.share_2_icon_16 },
                    { PrivacyType.Private, Resources.padlock_closed_icon_16 },
                };

            var ic = new ImageCollection();

            for (var i = 0; i < items.Count; i++)
            {
                ic.AddImage(items.ElementAt(i).Value);
                ri.Items.Add(new ImageComboBoxItem(items.ElementAt(i).Key.DisplayName, items.ElementAt(i).Key, i));
            }
            ri.SmallImages = ic;
            
            ri.Buttons.Clear();
            return ri;
        }
        public static RepositoryItemImageComboBox PrivacyEditorPg()
        {
            var ri = PrivacyEditor();
            ri.Name = Resources.DbField_Privacy + "Pg" + Resources.Label_Editor;
            ri.GlyphAlignment = HorzAlignment.Near;
            return ri;
        }

        public static RepositoryItemTextEdit TextEditor()
        {
            var ri = new RepositoryItemTextEdit { Name = Resources.Label_Text + Resources.Label_Editor };
            ri.KeyDown += TextEditor_KeyDown;
            return ri;
        }
        //public static RepositoryItemButtonEdit TransmittalLinkEditor()
        //{
        //    var ri = new RepositoryItemButtonEdit { Name = Resources.Label_TransmittalLink + Resources.Label_Editor };
        //    ri.Buttons.Clear();
        //    ri.Buttons.Add(new EditorButton(ButtonPredefines.Glyph, Resources.info_icon_16, null));

        //    ri.ButtonClick += TransmittalLinkEditor_ButtonClick;
        //    ri.DoubleClick += TransmittalLink_DoubleClick;
        //    ri.KeyDown += TextEditor_KeyDown;
        //    return ri;
        //}

        public static RepositoryItemComboBox TransmittalTargetEditor()
        {
            var ri = new RepositoryItemComboBox
            {
                Name = Resources.Label_TransmittalTarget + Resources.Label_Editor,
                CycleOnDblClick = true,
                NullText = Resources.Label_SelectEmail,
            };
            ri.Items.Clear();
            return ri;
        }


        private static void TransmittalLink_DoubleClick(object sender, EventArgs e)
        {
            var b = sender as ButtonEdit;
            if (b == null || b.EditValue == null) return;

            Clipboard.SetText(b.EditValue.ToString());
            Controller.Logger.Info(Resources.Msg_CellValueToClipboard);
        }

        public static void RefreshPartyLookupEditor(GridView gView)
        {
            var editor = (RepositoryItemLookUpEdit)gView.Columns[Resources.DbField_Client].ColumnEdit;

            editor.BeginUpdate();
            editor.DataSource = DataSourcePartyGrid.Invoke();
            editor.EndUpdate();
        }
        
        public static void RefreshPartyLookupEditor(PropertyGridControl gControl)
        {
            var row = (EditorRow)gControl.Rows["rowClient"];

            var editor = (RepositoryItemLookUpEdit)row.Properties.RowEdit;

            editor.BeginUpdate();
            editor.DataSource = DataSourcePartyGrid.Invoke();
            editor.EndUpdate();
        }

        public static void RefreshTransmittalTargetEditor(GridView gView, GridColumn col)
        {
            var items = ((Party)gView.GetFocusedRow())
                .Details
                .Where(x => Equals(x.DetailType, PartyDetailType.Email))
                .Select(x => x.MinorId)
                .ToList();

            var editor = (RepositoryItemComboBox)col.ColumnEdit;
            editor.Items.Clear();
            editor.Items.AddRange(items);
        }

        private static void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            var editor = (TextEdit)sender;
            var gControl = (GridControl)editor.Parent;
            var gView = (GridView)gControl.FocusedView;

            switch (e.KeyData)
            {
                case Keys.Alt | Keys.Enter:
                    if (editor.EditValue != null)
                    {
                        var text = editor.EditValue.ToString();
                        var selectionStart = editor.SelectionStart;
                        editor.EditValue = string.Format("{0}{1}{2}", 
                            text.Substring(0, editor.SelectionStart), 
                            Environment.NewLine, 
                            text.Substring(editor.SelectionStart));
                        editor.SelectionStart = selectionStart + Environment.NewLine.Length;
                        e.Handled = true;
                    }
                    break;

                case Keys.Escape:
                    gControl.FocusedView.HideEditor();
                    e.Handled = true;
                    break;

                case Keys.Right:
                case Keys.Left:
                    if (editor.SelectionLength == editor.Text.Length)
                    {
                        if (e.KeyData == Keys.Right) editor.SelectionStart = editor.Text.Length;
                        editor.SelectionLength = 0;
                        e.Handled = true;
                    }
                    if (editor.Text.Length != 0) return;
                    gControl.FocusedView.CloseEditor();
                    gView.FocusedColumn = e.KeyData == Keys.Right 
                        ? gView.Columns[(gView.FocusedColumn.VisibleIndex + 1)] 
                        : gView.Columns[(gView.FocusedColumn.VisibleIndex - 1)];
                    e.Handled = true;
                    break;
            }
        }

        //private static void TransmittalLinkEditor_ButtonClick(object sender, ButtonPressedEventArgs e)
        //{
        //    var b = sender as ButtonEdit;
        //    if (b != null && b.EditValue != null) Controller.WebLinkShortener.ReadStats(b.EditValue.ToString());
        //}
    }
}

