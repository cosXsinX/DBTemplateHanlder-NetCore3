﻿using System;
namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class ColumnModel : IColumnModel
    {
        public ColumnModel()
        {
        }

        public ColumnModel(string name,
                string type, bool isPrimaryKey)
        {
            Name = name;
            Type = type;
            IsPrimaryKey = isPrimaryKey;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsAutoGeneratedValue { get; set; }
        public bool IsNotNull { get; set; }
        public int ValueMaxSize { get; set; }
        public ITableModel ParentTable { get; set; }
        public bool IsIndexed { get; set; }
    }
}
