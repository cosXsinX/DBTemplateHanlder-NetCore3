using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public class TableDescriptor
    {
        public TableDescriptor(String TableNameStr)
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




        private readonly List<ColumnDescriptor> _ColumnsList =
                new List<ColumnDescriptor>();
        public List<ColumnDescriptor> get_ColumnsList()
        {
            return _ColumnsList;
        }

        public void AddColumn(ColumnDescriptor added)
        {
            if (added == null) return;
            _ColumnsList.Add(added);
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
        public DatabaseDescriptor ParentDatabase;
	
	    public void UpdateContainedColumnsParentReference()
        {
            foreach (ColumnDescriptor currentColumn in _ColumnsList)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
