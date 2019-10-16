using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public class TableDescriptionPOJO
    {
        public TableDescriptionPOJO(String TableNameStr)
        {
            _NameStr = TableNameStr;
        }

        private String _NameStr;

        public String get_NameStr()
        {
            return _NameStr;
        }

        public void set_NameStr(String _NameStr)
        {
            this._NameStr = _NameStr;
        }




        private readonly List<TableColumnDescriptionPOJO> _ColumnsList =
                new List<TableColumnDescriptionPOJO>();
        public List<TableColumnDescriptionPOJO> get_ColumnsList()
        {
            return _ColumnsList;
        }

        public void AddColumn(TableColumnDescriptionPOJO added)
        {
            if (added == null) return;
            _ColumnsList.add(added);
        }

        public int get_ColumnNumber()
        {
            return _ColumnsList.Count;
        }

        public void ClearColumns()
        {
            _ColumnsList.Clear();
        }

        //Template handler specific properties
        public DatabaseDescriptionPOJO ParentDatabase;
	
	    public void UpdateContainedColumnsParentReference()
        {
            for (TableColumnDescriptionPOJO currentColumn : _ColumnsList)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
