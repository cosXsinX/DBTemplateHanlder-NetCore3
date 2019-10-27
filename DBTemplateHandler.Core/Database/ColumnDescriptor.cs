﻿using System;
namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class ColumnDescriptor
    {
        public ColumnDescriptor(string name,
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
        //Template Handler specific properties
        public TableDescriptor ParentTable;
    }
}