namespace MakeConfig.Excel
{
    public struct ColumnMeta
    {
        public string Description;
        public string DefaultValue;
        public string Name;
        public string Constraint;
        public string Type;
        public string Tag;

        public override string ToString()
        {
            return $"{Description}\t{DefaultValue}\t{Name}\t{Constraint}\t{Type}\t{Tag}";
        }

        public static bool Equals(ColumnMeta left, ColumnMeta right, out string column)
        {
            if (left.Description != right.Description)
            {
                column = nameof(Description);
                return false;
            }
            column = null;
            return true;
        }
    }
}