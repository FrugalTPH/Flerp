using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraVerticalGrid.Rows;
using Flerp;
using Flerp.Client.Helpers;
using Flerp.Client.Properties;
using Flerp.Data;
using Flerp.DomainModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DevExpress.XtraGrid.Helpers
{
    internal static class XtraGridExtensions
    {
        private static GridHitInfo _downHitInfo;


        private static void gView_DoubleClick(object sender, EventArgs e)
        {
            var gView = (GridView)sender;

            if (gView.FocusedColumn.FieldName != Resources.DbField_Id && gView.FocusedColumn.FieldName != Resources.DbField_MajorId) return;
            var id = (string)gView.GetFocusedRowCellValue(gView.FocusedColumn);

            IPersistable entity;
            if (!Controller.TryGetEntityById(id, out entity)) return;

            entity.IdDoubleClick();
        }

        private static void gView_KeyDown(object sender, KeyEventArgs e)
        {
            var gView = (GridView)sender;

            if ((gView.FocusedColumn.FieldName == Resources.DbField_Id || gView.FocusedColumn.FieldName == Resources.DbField_MajorId) && e.KeyCode == Keys.Return) gView_DoubleClick(gView, e);

            var cols = gView.Columns
                        .Where(x => x.OptionsColumn.TabStop && x.VisibleIndex >= 0)
                        .OrderBy(x => x.VisibleIndex);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (gView.FocusedRowHandle == 0 && gView.FocusedColumn == cols.First())
                    {
                        gView.FocusedColumn = cols.Last();
                        gView.FocusedRowHandle = gView.RowCount - 1;
                        e.Handled = true;
                    }
                    break;
                case Keys.Tab:
                case Keys.Right:
                    if (gView.FocusedRowHandle == gView.RowCount - 1 && gView.FocusedColumn == cols.Last())
                    {
                        gView.FocusedColumn = cols.First();
                        gView.FocusedRowHandle = 0;
                        e.Handled = true;
                    }
                    break;
            }
        }

        internal static T XtraGridGetEntity<T>(this GridControl control) where T : IPersistable
        {
            var gView = (GridView)control.FocusedView;
            return (T)gView.GetFocusedRow();
        }

        internal static void XtraGridShowEditor(this GridView gView, IPersistable entity, string column)
        {
            gView.GridControl.PerformSafely(() =>
            {
                var row = -1;
                for (var i = 0; i < gView.RowCount; i++) if (gView.GetRow(i) == entity) row = i;
                gView.FocusedRowHandle = row;
                gView.FocusedColumn = gView.Columns[column] ?? gView.Columns.OrderBy(x => x.VisibleIndex).First(x => x.OptionsColumn.TabStop && x.VisibleIndex >= 0);
                gView.ShowEditor();
            });
        }

        internal static void XtraGridRefresh(this GridControl control, object source, bool saveLayout)
        {
            control.PerformSafely(() => 
            {
                var gView = (GridView)control.FocusedView;
                var helper = new RefreshHelper(gView, Resources.DbField_Id);

                try
                {
                    if (saveLayout) helper.SaveViewInfo();
                    control.BeginUpdate();
                    control.DataSource = source;
                }
                finally
                {
                    control.EndUpdate();
                    if (saveLayout) helper.LoadViewInfo();
                }
            });                   
        }

        internal static void XtraGridSaveLayout(this GridView gView, string formName, IRepository<AppSetting> appSettingStore)
        {
            var str = new MemoryStream();
            gView.SaveLayoutToStream(str);
            str.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(str);

            appSettingStore.Save(new AppSetting(Resources.Layout_Prefix + formName + Resources.Layout_Delim + gView.Name, reader.ReadToEnd()));
        }

        internal static void XtraGridRestoreLayout(this GridView gView, string formName, IRepository<AppSetting> appSettingStore)
        {
            var layout = appSettingStore.GetById(Resources.Layout_Prefix + formName + Resources.Layout_Delim + gView.Name);
            if (layout == null) return;

            var byteArray = Encoding.ASCII.GetBytes(layout.Value);
            var stream = new MemoryStream(byteArray);
            gView.RestoreLayoutFromStream(stream);
        }

        internal static void XtragridInitializeColumns(this GridView gView, IEnumerable<ColumnDefinition> colDefs)
        {
            gView.Columns.Clear();
            
            int i = 0;
            foreach (var def in colDefs)
            {
                var col = gView.Columns.AddVisible(def.PropertyName);

                col.ColumnEdit = def.Ri;
                
                col.VisibleIndex = def.IsHidden ? -i++ : i++;

                if (def.IsReadOnly)
                {
                    col.ColumnEdit.ReadOnly = true;
                    col.OptionsColumn.TabStop = false;
                    col.AppearanceHeader.ForeColor = Color.FromArgb(154, 154, 166);
                    col.AppearanceCell.ForeColor = Color.FromArgb(154, 154, 166);
                }

                if (def.MinWidth > 0) col.MinWidth = def.MinWidth;
                if (def.MaxWidth > 0) col.MaxWidth = def.MaxWidth;
                if (def.Caption != null) col.Caption = def.Caption;
                if (def.ToolTip != null) col.ToolTip = def.ToolTip;

                if (def.PropertyName == Resources.DbField_Id || def.PropertyName == Resources.DbField_MajorId)
                {
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.TabStop = true;
                }
                col.OptionsFilter.FilterPopupMode = !col.FieldName.Contains(Resources.Label_Date) 
                    ? FilterPopupMode.CheckedList 
                    : FilterPopupMode.Date;
            }
        }

        internal static void XtragridSetOptions(this GridView gView, bool allowIEnumerableDetails)
        {
            gView.DataController.AllowIEnumerableDetails = allowIEnumerableDetails;

            gView.GridControl.ShowOnlyPredefinedDetails = true;

            gView.OptionsView.RowAutoHeight = true;
            gView.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;

            gView.OptionsView.ColumnAutoWidth = false;

            gView.OptionsDetail.ShowDetailTabs = false;

            gView.OptionsSelection.MultiSelect = true;
            gView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

            gView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowForFocusedRow;

            gView.OptionsView.AllowGlyphSkinning = true;

            gView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDownFocused;

            gView.OptionsNavigation.UseTabKey = true;

            gView.GroupPanelText = gView.ViewCaption;

            gView.DoubleClick += gView_DoubleClick;
            gView.KeyDown += gView_KeyDown;
            gView.RowUpdated += (sender, e) => ((IPersistable)e.Row).Save();
            //gView.CellValueChanged += (sender, e) => ((IPersistable)gView.GetRow(e.RowHandle)).Save();
        }

        private static string[] XtragridGetDragData(this ColumnView gView)
        {
            var selection = gView.GetSelectedRows();
            if (selection == null) return null;

            var count = selection.Length;
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = gView.GetRowCellDisplayText(selection[i], gView.Columns[Resources.DbField_Id]);
            }
            return result;
        }

        internal static void Xtragrid_MouseDown(this GridView gView, MouseEventArgs e)
        {
            _downHitInfo = gView.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None || e.Button != MouseButtons.Left || !_downHitInfo.InRowCell) _downHitInfo = null;
        }

        internal static void Xtragrid_MouseMove(this GridControl gControl, MouseEventArgs e)
        {
            var complete = false;

            try
            {
                if (e.Button != MouseButtons.Left || _downHitInfo == null) return;

                var dragSize = SystemInformation.DragSize;
                var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2, _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (dragRect.Contains(new Point(e.X, e.Y))) return;

                var gView = (GridView)gControl.FocusedView;

                gControl.DoDragDrop(gView.XtragridGetDragData(), DragDropEffects.All);

                complete = true;
            }
            finally
            {
                if (!complete) _downHitInfo = null;
            }
        }

        internal static void Xtragrid_ShowingEditor(this GridView gView)
        {
            var col = gView.FocusedColumn;

            if (col.FieldName == Resources.DbField_Category) RiProvider.RefreshCategoryLookupEditor(gView, col);
            else if (col.FieldName == Resources.Label_TransmittalTarget) RiProvider.RefreshTransmittalTargetEditor(gView, col);
        }

        internal static void Xtragrid_ValidatingEditor(this GridView gView, IPersistable entity, BaseContainerValidateEditorEventArgs e)
        {
            if (gView.FocusedColumn.FieldName == "Value" && entity is PartyDetail)
            {
                e.Valid = PartyDetailType.IsValid(e.Value.ToString(), ((PartyDetail)entity).DetailType);
            }
        }
    }
}

namespace DevExpress.XtraTabbedMdi.Helpers
{
    internal static class XtraMdiTabExtensions
    {
        internal static XtraMdiTabPage XtraMdiTabGet(this XtraTabbedMdiManager manager, string id)
        {
            var dockedPage = manager
                .Pages
                .Cast<XtraMdiTabPage>()
                .FirstOrDefault(x => id == x.Text);

            if (dockedPage != null) return dockedPage;

            var floatForm = manager
                .FloatForms
                .FirstOrDefault(x => x.Name == id);
            return floatForm != null 
                ? manager.Pages[floatForm] 
                : null;
        }
    }
}

namespace DevExpress.XtraVerticalGrid.Helpers
{
    internal static class XtraVerticalGridExtensions
    {
        internal static void XtraVerticalGridPopulate(this PropertyGridControl control, object source)
        {
            control.PerformSafely(() =>
            {
                try
                {
                    control.BeginUpdate();
                    control.SelectedObject = source;
                }
                finally
                {
                    control.EndUpdate();
                }
            });
        }

        internal static void XtraVerticalGridInitializeRows(this PropertyGridControl control, IEnumerable<ColumnDefinition> colDefs)
        {
            control.Rows.Clear();

            foreach (var def in colDefs)
            {
                var row = new EditorRow(def.PropertyName);

                row.Properties.RowEdit = def.Ri;
                row.Visible = !def.IsHidden;
                row.Properties.ReadOnly = def.IsReadOnly;
                row.Properties.Caption = def.Caption ?? def.PropertyName;
                row.Properties.ToolTip = def.ToolTip;

                control.Rows.Add(row);
            }
        }

        internal static void XtraVerticalGridSetOptions(this PropertyGridControl control)
        {
            control.OptionsBehavior.PropertySort = PropertySort.NoSort;
            control.BestFit();
        }
    }
}


