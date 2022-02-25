using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
using System.Linq;

namespace Flerp.Client.Helpers
{
    public class RefreshHelper
    {
        [Serializable]
        protected struct RowInfo
        {
            public object Id;
            public int Level;
        };

        private readonly GridView _view;
        private readonly string _keyFieldName;
        private ArrayList _saveExpList;
        private ArrayList _saveSelList;
        private ArrayList _saveMasterRowsList;
        private int _visibleRowIndex = -1;

        public RefreshHelper(GridView view, string keyFieldName)
        {
            _view = view;
            _keyFieldName = keyFieldName;
        }

        private ArrayList SaveExpList
        {
            get { return _saveExpList ?? (_saveExpList = new ArrayList()); }
        }

        private ArrayList SaveSelList
        {
            get { return _saveSelList ?? (_saveSelList = new ArrayList()); }
        }

        private ArrayList SaveMasterRowsList
        {
            get { return _saveMasterRowsList ?? (_saveMasterRowsList = new ArrayList()); }
        }

        protected int FindParentRowHandle(RowInfo rowInfo, int rowHandle)
        {
            var result = _view.GetParentRowHandle(rowHandle);
            while (_view.GetRowLevel(result) != rowInfo.Level)
                result = _view.GetParentRowHandle(result);
            return result;
        }

        protected void ExpandRowByRowInfo(RowInfo rowInfo)
        {
            var dataRowHandle = _view.LocateByValue(0, _view.Columns[_keyFieldName], rowInfo.Id);
            if (dataRowHandle == GridControl.InvalidRowHandle) return;

            var parentRowHandle = FindParentRowHandle(rowInfo, dataRowHandle);
            _view.SetRowExpanded(parentRowHandle, true, false);
        }

        protected int GetRowHandleToSelect(RowInfo rowInfo)
        {
            var dataRowHandle = _view.LocateByValue(0, _view.Columns[_keyFieldName], rowInfo.Id);

            return _view.GetRowLevel(dataRowHandle) != rowInfo.Level 
                ? FindParentRowHandle(rowInfo, dataRowHandle) 
                : dataRowHandle;
        }

        protected void SelectRowByRowInfo(RowInfo rowInfo, bool isFocused)
        {
            if (isFocused)
                _view.FocusedRowHandle = GetRowHandleToSelect(rowInfo);
            else
                _view.SelectRow(GetRowHandleToSelect(rowInfo));
        }

        private void SaveSelectionViewInfo(IList list)
        {
            list.Clear();
            var column = _view.Columns[_keyFieldName];
            RowInfo rowInfo;

            var selectionArray = _view.GetSelectedRows();

            if (selectionArray != null)  // otherwise we have a single focused but not selected row

                foreach (var t in selectionArray)
                {
                    var dataRowHandle = t;
                    rowInfo.Level = _view.GetRowLevel(dataRowHandle);
                    if (dataRowHandle < 0) // group row
                        dataRowHandle = _view.GetDataRowHandleByGroupRowHandle(dataRowHandle);
                    rowInfo.Id = _view.GetRowCellValue(dataRowHandle, column);
                    list.Add(rowInfo);
                }

            rowInfo.Id = _view.GetRowCellValue(_view.FocusedRowHandle, column);

            rowInfo.Level = _view.GetRowLevel(_view.FocusedRowHandle);

            list.Add(rowInfo);
        }

        private void SaveExpansionViewInfo(IList list)
        {
            if (_view.GroupedColumns.Count == 0) return;
            list.Clear();
            var column = _view.Columns[_keyFieldName];
            for (var i = -1; i > int.MinValue; i--)
            {
                if (!_view.IsValidRowHandle(i)) break;
                if (!_view.GetRowExpanded(i)) continue;

                RowInfo rowInfo;
                var dataRowHandle = _view.GetDataRowHandleByGroupRowHandle(i);
                rowInfo.Id = _view.GetRowCellValue(dataRowHandle, column);
                rowInfo.Level = _view.GetRowLevel(i);
                list.Add(rowInfo);
            }
        }

        private void SaveExpandedMasterRows(IList list)
        {
            if (_view.GridControl.Views.Count == 1) return;
            list.Clear();
            var column = _view.Columns[_keyFieldName];
            for (var i = 0; i < _view.DataRowCount; i++)
                if (_view.GetMasterRowExpanded(i))
                    list.Add(_view.GetRowCellValue(i, column));
        }

        private void SaveVisibleIndex()
        {
            _visibleRowIndex = _view.GetVisibleIndex(_view.FocusedRowHandle) - _view.TopRowIndex;
        }

        private void LoadVisibleIndex()
        {
            _view.MakeRowVisible(_view.FocusedRowHandle, true);
            _view.TopRowIndex = _view.GetVisibleIndex(_view.FocusedRowHandle) - _visibleRowIndex;
        }

        private void LoadSelectionViewInfo(IList list)
        {
            _view.BeginSelection();
            try
            {
                _view.ClearSelection();
                for (var i = 0; i < list.Count; i++)
                    SelectRowByRowInfo((RowInfo)list[i], i == list.Count - 1);
            }
            finally
            {
                _view.EndSelection();
            }
        }

        private void LoadExpansionViewInfo(IEnumerable list)
        {
            if (_view.GroupedColumns.Count == 0) return;
            _view.BeginUpdate();
            try
            {
                _view.CollapseAllGroups();
                foreach (RowInfo info in list)
                    ExpandRowByRowInfo(info);
            }
            finally
            {
                _view.EndUpdate();
            }
        }

        private void LoadExpandedMasterRows(IEnumerable list)
        {
            _view.BeginUpdate();
            try
            {
                _view.CollapseAllDetails();
                var column = _view.Columns[_keyFieldName];
                foreach (var rowHandle in list.Cast<object>().Select(t => _view.LocateByValue(0, column, t)))
                {
                    _view.SetMasterRowExpanded(rowHandle, true);
                }
            }
            finally
            {
                _view.EndUpdate();
            }
        }

        public void SaveViewInfo()
        {
            SaveExpandedMasterRows(SaveMasterRowsList);
            SaveExpansionViewInfo(SaveExpList);
            SaveSelectionViewInfo(SaveSelList);
            SaveVisibleIndex();
        }

        public void LoadViewInfo()
        {
            LoadExpandedMasterRows(SaveMasterRowsList);
            LoadExpansionViewInfo(SaveExpList);
            LoadSelectionViewInfo(SaveSelList);
            LoadVisibleIndex();    
        }
    }
}