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
            if (left.Name != right.Name)
            {
                column = nameof(Name);
                return false;
            }
            if (left.Constraint != right.Constraint)
            {
                column = nameof(Constraint);
                return false;
            }
            if (left.Type != right.Type)
            {
                column = nameof(Type);
                return false;
            }
            if (left.Tag != right.Tag)
            {
                column = nameof(Tag);
                return false;
            }
            column = null;
            return true;
        }
    }

}