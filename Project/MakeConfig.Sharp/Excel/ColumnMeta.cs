namespace MakeConfig.Excel
{

    public struct ColumnMeta
    {
        public string Description;
        public string DefaultValue;
        public string FieldFullName;
        public string Constraint;
        public string TypeSpec;
        public string Tag;

        public override string ToString()
        {
            return $"{Description}\t{DefaultValue}\t{FieldFullName}\t{Constraint}\t{TypeSpec}\t{Tag}";
        }

        public static bool Equals(ColumnMeta left, ColumnMeta right, out string column)
        {
            if (left.FieldFullName != right.FieldFullName)
            {
                column = nameof(FieldFullName);
                return false;
            }
            if (left.Constraint != right.Constraint)
            {
                column = nameof(Constraint);
                return false;
            }
            if (left.TypeSpec != right.TypeSpec)
            {
                column = nameof(TypeSpec);
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